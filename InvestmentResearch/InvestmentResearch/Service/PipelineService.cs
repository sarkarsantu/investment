using InvestmentResearch.Helper;
using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace InvestmentResearch.Service
{
    public class PipelineService : IPipelineService
    {
        private readonly IRssFeedDatabaseService _rssFeedDatabaseService;
        private readonly IFeedProcessorService _feedProcessorService;
        private readonly IAICall _AICall;
        private readonly IGeminiService _geminiService;
        private readonly IFileHelper _fileHelper;
        private readonly AppSettings _appSettings;
        private readonly IEmailService _emailService;
        private List<TrackBySector> _trackBySectors = new ();
        private List<string> logs = new List<string>();

        public PipelineService(
            IRssFeedDatabaseService rssFeedDatabaseService,
            IFeedProcessorService feedProcessorService,
            IAICall dailyAICall,
            IGeminiService geminiService,
            IFileHelper fileHelper,
            IOptions<AppSettings> appSettings,
            IEmailService emailService)
        {
            _rssFeedDatabaseService = rssFeedDatabaseService;
            _feedProcessorService = feedProcessorService;
            _AICall = dailyAICall;
            _geminiService = geminiService;
            _fileHelper = fileHelper;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
            _emailService = emailService;
        }

        public async Task RunRSSFeedAsync()
        {
            // Load RSS feeds from database
            string log = string.Empty;
            var rssFeeds = await _rssFeedDatabaseService.GetAllFeedsAsync();

            if (rssFeeds == null || !rssFeeds.Any())
            {
                log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## No RSS feeds configured in database.";
                Console.WriteLine(log);
                logs.Add(log);
                return;
            }
            var sectors = rssFeeds.GroupBy(comparer => comparer.Theme)
                                  .Select(group => new { Key = group.Key, Feeds = group.ToList() })
                                  .ToList();

            foreach (var sector in sectors)
            {
                TrackBySector trackBySector = new()
                {
                    Sector = sector.Key,
                    LinkCount = 0,
                    FinalLinkCount = 0,
                    MaxRank = 0,
                    GoogleAlerts = new List<TrackByGoogleAlert>()
                };
                
                var googleAlters = rssFeeds.Where(p => p.Theme == sector.Key).ToList();
                if (sector.Key.ToLower() == "Watchlist".ToLower())
                {
                    foreach (var googleAlter in googleAlters)
                    {
                        TrackByGoogleAlert trackByGoogleAlert = new()
                        {
                            GoogleAlertName = googleAlter.Name ?? string.Empty,
                            LinkCount = 0,
                            FinalLinkCount = 0
                        };
                        log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector.Key} :- {googleAlter.Name.Trim() ?? string.Empty} is getting feed links.";
                        Console.WriteLine(log);
                        logs.Add(log);
                        var resultLinks = _feedProcessorService.ProcessFeed(googleAlter, 20);
                        if (resultLinks != null && resultLinks.Count > 0)
                        {
                            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector.Key} :- {googleAlter.Name.Trim() ?? string.Empty} finds {resultLinks.Count} links.";
                            Console.WriteLine(log);
                            logs.Add(log);
                            trackByGoogleAlert.LinkCount = resultLinks.Count;
                            trackBySector.LinkCount += trackByGoogleAlert.LinkCount;
                            (trackByGoogleAlert.FinalLinkCount, trackBySector.MaxRank) = await ProcesRSSFeed(sector.Key, resultLinks, googleAlter.Name);
                            trackBySector.FinalLinkCount += trackByGoogleAlert.FinalLinkCount;
                        }
                        else
                        {
                            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector.Key} :- {googleAlter.Name.Trim() ?? string.Empty} does not find any links.";
                            Console.WriteLine(log);
                            logs.Add(log);
                        }
                        trackBySector.GoogleAlerts.Add(trackByGoogleAlert);
                    }
                    this._trackBySectors.Add(trackBySector);
                }
                else
                {
                    List<Link> links = new();
                    foreach (var googleAlter in googleAlters)
                    {
                        TrackByGoogleAlert trackByGoogleAlert = new()
                        {
                            GoogleAlertName = googleAlter.Name ?? string.Empty,
                            LinkCount = 0,
                            FinalLinkCount = 0
                        };
                        var resultLinks = _feedProcessorService.ProcessFeed(googleAlter, 20);
                        if (resultLinks != null && resultLinks.Count > 0)
                        {
                            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector.Key} :- {googleAlter.Name.Trim() ?? string.Empty} finds {resultLinks.Count.ToString()}.";
                            Console.WriteLine(log);
                            logs.Add(log);
                            trackByGoogleAlert.LinkCount = resultLinks.Count;
                            trackByGoogleAlert.FinalLinkCount = resultLinks.Count;
                            trackBySector.LinkCount += trackByGoogleAlert.LinkCount;
                            links.AddRange(resultLinks);
                        }
                        else
                        {
                            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector.Key} :- {googleAlter.Name.Trim() ?? string.Empty} does not find any links.";
                            Console.WriteLine(log);
                            logs.Add(log);
                        }
                        trackBySector.GoogleAlerts.Add(trackByGoogleAlert);
                    }

                    (trackBySector.FinalLinkCount, trackBySector.MaxRank) = await ProcesRSSFeed(sector.Key, links, null);
                    this._trackBySectors.Add(trackBySector);
                    links = new();
                }
                
            }
            // send the summary email after processing all sectors
            await SendSummaryEmailAsync(_feedProcessorService.GetUnknownDomains());
        }

        private async Task<(int, int)> ProcesRSSFeed(string sector, List<Link> links, string? companyName = null)
        {
            string log = string.Empty;
            if (links.Count == 0)
            {
                log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {companyName.Trim() ?? string.Empty} does not find any links.";
                Console.WriteLine(log);
                logs.Add(log);
                return (links.Count, 0);
            }
            links = this._feedProcessorService.UpdateLinksBySLNo(links);

            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {(string.IsNullOrWhiteSpace(companyName) ? sector.Trim() : companyName.Trim())} is removing unrelated feed links.";
            Console.WriteLine(log);
            logs.Add(log);
            var modifiedLinks = await FilterModifiedLinks(
                    sector,
                    companyName,
                    "RSSFeed_System_Relevant_" + sector + ".txt",
                    "RSSFeed_User_Relevant_" + sector + ".txt",
                    links);
            if (modifiedLinks == null || modifiedLinks?.Count == 0)
            {
                return (modifiedLinks?.Count ?? 0, 0);
            }

            var relevanceLink = modifiedLinks;
            if (relevanceLink == null || relevanceLink.Count() == 0)
            {
                return (relevanceLink?.Count() ?? 0, 0);
            }
            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {(string.IsNullOrWhiteSpace(companyName) ? sector.Trim() : companyName.Trim())} is marking duplicate feed links.";
            Console.WriteLine(log);
            var relevanceLinkWithRank = await FilterModifiedLinks(
                    sector,
                    companyName,
                    "RSSFeed_System_Duplicate_" + sector + ".txt",
                    "RSSFeed_User_Duplicate_" + sector + ".txt",
                    links);

            var finalLinks = relevanceLinkWithRank?.OrderBy(p => p.Rank);
            if (finalLinks == null || finalLinks.Count() == 0)
            {
                return (finalLinks?.Count() ?? 0, 0);
            }
            int maxRank = finalLinks.Max(p => p.Rank);

            // Instead of saving an HTML file, send emails with links (20 per email)
            var finalLinksList = finalLinks.ToList();

            string targetEmail = "santusarkar.stockmarket@gmail.com";
            int linksPerEmail = 20;
            int totalEmails = (finalLinksList.Count + linksPerEmail - 1) / linksPerEmail; // Ceiling division

            for (int emailIndex = 0; emailIndex < totalEmails; emailIndex++)
            {
                var emailLinks = finalLinksList.Skip(emailIndex * linksPerEmail).Take(linksPerEmail).ToList();

                // Generate HTML body using the same format as GenerateRSSFeedHtml
                string emailBody = HtmlGenerator.GenerateRSSFeedHtml(emailLinks, sector);

                string subject = $"{(string.IsNullOrWhiteSpace(companyName) ? sector : companyName)} - {DateTime.Now:yyyy-MM-dd} (Part {emailIndex + 1} of {totalEmails} Total links - {emailLinks.Count})";

                try
                {
                    log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {(string.IsNullOrWhiteSpace(companyName) ? sector.Trim() : companyName)} is sending {emailIndex + 1}/{totalEmails} sent with {emailLinks.Count} links.";
                    Console.WriteLine(log);
                    logs.Add(log);
                    await _emailService.SendEmailAsync(targetEmail, subject, emailBody);
                }
                catch (Exception ex)
                {
                    log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {(string.IsNullOrWhiteSpace(companyName) ? sector.Trim() : companyName)} fails to send email {emailIndex + 1} : {ex.Message}";
                    Console.WriteLine(log);
                    logs.Add(log);
                }
            }

            log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {sector} :- {(string.IsNullOrWhiteSpace(companyName) ? sector.Trim() : companyName)} completes.";
            Console.WriteLine(log);
            logs.Add(log);
            return (finalLinksList?.Count ?? 0, maxRank);
        }

        private async Task<List<Link>?> FilterModifiedLinks(string sectorName, string? companyName, string systemInstructionfileName, string userDatafileName, List<Link> links)
        {
            string log = string.Empty;
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var jsonContent = JsonSerializer.Serialize<List<Link>>(links, options);
            var systemInstruction = await this._fileHelper.GetPrompt(systemInstructionfileName, sectorName: sectorName, companyName: companyName, InputData: jsonContent);
            var userData = await this._fileHelper.GetPrompt(userDatafileName, sectorName: sectorName, companyName: companyName, InputData: jsonContent);
            var jsonPayload = await _geminiService.RSSFeedAsync(systemInstruction: systemInstruction, userData: userData);

            try
            {
                return JsonSerializer.Deserialize<List<Link>>(jsonPayload);
            }
            catch (JsonException ex)
            {
                log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## JSON deserialization failed. Payload: {jsonPayload.Substring(0, Math.Min(200, jsonPayload.Length))}...";
                Console.WriteLine(log);
                return links;
            }
        }

        public async Task RunPromptAsync()
        {
            var dailyContent = new List<CompanyResult>();
            var weeklyContent = new List<CompanyResult>();
            var monthlyContent = new List<CompanyResult>();

            foreach (var prompt in _appSettings.Prompts)
            {
                if (prompt.DailyCall)
                {
                    var contents = await _AICall.GenerateContent(prompt);
                    if (contents != null && contents.Count > 0)
                    {
                        dailyContent.AddRange(contents);
                    }
                }
                else if (prompt.WeeklyCall && DateTime.Today.DayOfWeek == DayOfWeek.Friday)
                {
                    var contents = await _AICall.GenerateContent(prompt);
                    if (contents != null && contents.Count > 0)
                    {
                        dailyContent.AddRange(contents);
                    }
                }
                else
                {
                    if (DateTime.Today.Day == 1)
                    {
                        var contents = await _AICall.GenerateContent(prompt);
                        if (contents != null && contents.Count > 0)
                        {
                            dailyContent.AddRange(contents);
                        }
                    }
                }
            }

            if (dailyContent.Count != 0)
            {
                string htmlContent = HtmlGenerator.GenerateHtml(dailyContent);

                await _fileHelper.FileSave("dailysync.html", htmlContent);
                await _fileHelper.UploadtoGitHub("dailysync.html");
            }
            if (weeklyContent.Count != 0)
            {
                string htmlContent = HtmlGenerator.GenerateHtml(weeklyContent);

                await _fileHelper.FileSave("weeklysync.html", htmlContent);
                await _fileHelper.UploadtoGitHub("weeklysync.html");
            }
            if (monthlyContent.Count != 0)
            {
                string htmlContent = HtmlGenerator.GenerateHtml(monthlyContent);

                await _fileHelper.FileSave("monthlysync.html", htmlContent);
                await _fileHelper.UploadtoGitHub("monthlysync.html");
            }

            Console.WriteLine("\nResearch completed successfully!");
        }

        private async Task SendSummaryEmailAsync(string UnknownDomains)
        {
            string log = string.Empty;
            try
            {
                if (_trackBySectors == null || _trackBySectors.Count == 0)
                {
                    log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## No sectors processed, skipping summary email.";
                    logs.Add(log);
                    return;
                }

                string htmlBody = HtmlGenerator.GenerateSummarySectorHtml(_trackBySectors, UnknownDomains, logs);
                string subject = $"Investment Research Summary - {DateTime.Now:yyyy-MM-dd}";
                string targetEmail = "santusarkar.stockmarket@gmail.com";

                await _emailService.SendEmailAsync(targetEmail, subject, htmlBody);
                log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## Summary email sent successfully.";
                Console.WriteLine(log);
                logs.Add(log);
            }
            catch (Exception ex)
            {
                log = $"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## Failed to send summary email: {ex.Message}";  
                Console.WriteLine(log);
                logs.Add(log);
            }
        }


        public async void RunAsync()
        {
            await RunRSSFeedAsync();

            //await RunPromptAsync();
        }
    }
}
