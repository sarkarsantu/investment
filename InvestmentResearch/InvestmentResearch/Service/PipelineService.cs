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

        public PipelineService(
            IRssFeedDatabaseService rssFeedDatabaseService, 
            IFeedProcessorService feedProcessorService,
            IAICall dailyAICall,
            IGeminiService geminiService,
            IFileHelper fileHelper, 
            IOptions<AppSettings> appSettings)
        {
            _rssFeedDatabaseService = rssFeedDatabaseService;
            _feedProcessorService = feedProcessorService;
            _AICall = dailyAICall;
            _geminiService = geminiService;
            _fileHelper = fileHelper;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task RunRSSFeedAsync()
        {
            // Load RSS feeds from database
            var rssFeeds = await _rssFeedDatabaseService.GetAllFeedsAsync();

            if (rssFeeds == null || !rssFeeds.Any())
            {
                Console.WriteLine("No RSS feeds configured in database.");
                return;
            }

            var sectors = rssFeeds.GroupBy(comparer => comparer.Theme)
                                  .Select(group => new { Key = group.Key, Feeds = group.ToList() })
                                  .ToList();

            foreach( var sector in sectors)
            {
                Console.WriteLine($"Sector: {sector.Key}");
                List<Link> links = new();

                var sectorGoogleAlters = rssFeeds.Where(p => p.Theme == sector.Key).ToList();
                foreach (var googleAlter in sectorGoogleAlters)
                {
                    var resultLinks = _feedProcessorService.ProcessFeed(googleAlter, 20);
                    if (resultLinks != null && resultLinks.Count > 0)
                    {
                        links.AddRange(resultLinks);
                    }
                }
                for (int i = 0; i < links.Count; i++)
                {
                    links[i].SlNo = i + 1;
                }
                
                var modifiedLinks = await FilterModifiedLinks(
                        sector.Key, 
                        "RSSFeed_System_Relevant_" + sector.Key + ".txt", 
                        "RSSFeed_User_Relevant_" + sector.Key + ".txt", 
                        links);
                if (modifiedLinks == null || modifiedLinks?.Count == 0)
                {
                    continue;
                }

                var relevanceLink = modifiedLinks?.Where(p => p.IsRelevant = true);
                if (relevanceLink == null || relevanceLink.Count() == 0)
                {
                    continue;
                }
                var relevanceLinkWithRank = await FilterModifiedLinks(
                        sector.Key,
                        "RSSFeed_System_Duplicate_" + sector.Key + ".txt",
                        "RSSFeed_User_Duplicate_" + sector.Key + ".txt",
                        links);

                var finalLinks = relevanceLinkWithRank?.OrderBy(p => p.Rank);
                if (finalLinks == null || finalLinks.Count() == 0)
                {
                    continue;
                }

                // Generate and save HTML file for the sector
                var finalLinksList = finalLinks.ToList();
                string htmlContent = HtmlGenerator.GenerateRSSFeedHtml(finalLinksList, sector.Key);
                string fileName = $"{sector.Key}.html";
                await _fileHelper.FileSave(fileName, htmlContent);
                await _fileHelper.UploadtoGitHub(fileName);
                
                Console.WriteLine($"HTML file generated: {fileName}");
            }
        }

        private async Task<List<Link>?> FilterModifiedLinks(string sectorName, string systemInstructionfileName, string userDatafileName, List<Link> links)
        {
            var options = new JsonSerializerOptions { Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping };
            var jsonContent = JsonSerializer.Serialize<List<Link>>(links, options);
            var systemInstruction = await this._fileHelper.GetPrompt(systemInstructionfileName, sectorName: sectorName, companyName: null!, InputData: jsonContent);
            var userData = await this._fileHelper.GetPrompt(userDatafileName, sectorName: sectorName, companyName: null!, InputData: jsonContent);
            var jsonPayload = await _geminiService.RSSFeedAsync(systemInstruction: systemInstruction, userData: userData);

            // Clean up the JSON response
            //jsonPayload = CleanJsonResponse(jsonPayload);

            try
            {
                return JsonSerializer.Deserialize<List<Link>>(jsonPayload);
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"JSON deserialization failed. Payload: {jsonPayload.Substring(0, Math.Min(200, jsonPayload.Length))}...");
                throw;
            }
        }

        private string CleanJsonResponse(string response)
        {
            // Remove various markdown code block patterns
            response = System.Text.RegularExpressions.Regex.Replace(response, @"```(json|javascript|js)?\s*|\s*```", "");
            
            // Remove single backticks and other markdown formatting
            response = System.Text.RegularExpressions.Regex.Replace(response, @"`+", "");
            
            // Remove any leading/trailing non-JSON characters
            response = System.Text.RegularExpressions.Regex.Replace(response, @"^[^\[\{]*", "");
            response = System.Text.RegularExpressions.Regex.Replace(response, @"[^\]\}]*$", "");
            
            return response.Trim();
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

        public async void RunAsync()
        {
            await RunRSSFeedAsync();

            //await RunPromptAsync();
        }
    }
}
