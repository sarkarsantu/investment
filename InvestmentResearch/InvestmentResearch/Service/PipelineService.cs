using InvestmentResearch.Helper;
using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using Microsoft.Extensions.Options;

namespace InvestmentResearch.Service
{
    public class PipelineService : IPipelineService
    {
        private readonly IAICall _AICall;
        private readonly IFileHelper _fileHelper;
        private readonly AppSettings _appSettings;

        public PipelineService(IAICall dailyAICall, IFileHelper fileHelper, IOptions<AppSettings> appSettings)
        {
            _AICall = dailyAICall;
            _fileHelper = fileHelper;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async void Run()
        {
            var dailyContent = new List<CompanyResult>();
            var weeklyContent = new List<CompanyResult>();
            var monthlyContent = new List<CompanyResult>();

            foreach (var prompt in _appSettings.Prompts)
            {
                if (prompt.DailyCall)
                {
                    var contents = await _AICall.GenerateContent(prompt);
                    if(contents != null && contents.Count > 0)
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

            if(dailyContent.Count != 0)
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
    }
}
