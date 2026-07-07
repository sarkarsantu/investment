using InvestmentResearch.Model;
using InvestmentResearch.Service;
using Microsoft.Extensions.Options;

namespace InvestmentResearch.Repository
{
    public class FileHelper : IFileHelper
    {
        private const string _outputFolder = "Output";
        private const string _promptFolder = "Prompt";
        private readonly IGitHubHelper _gitHubHelper;
        private readonly AppSettings _appSettings;

        public FileHelper(IGitHubHelper gitHubHelper, IOptions<AppSettings> appSettings)
        {
            if (!Directory.Exists(_outputFolder))
            {
                Directory.CreateDirectory(_outputFolder);
            }

            _gitHubHelper = gitHubHelper;
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public async Task UploadtoGitHub(string fileName)
        {
            var filePath = Path.GetFullPath(_outputFolder + "\\" + fileName);
            string gitHubPath = $"{_appSettings.GitHubFolder}/{fileName}";
            bool ret =  await _gitHubHelper.UploadFileAsync(filePath, gitHubPath);
            Console.WriteLine("\n✓ All files uploaded to GitHub successfully!");

            Console.WriteLine("\nResearch completed successfully!");
        }

        public async Task FileSave(string fileName, string fileContent)
        {
            string outputFilePath = Path.Combine(_outputFolder, fileName);

            await File.WriteAllTextAsync(outputFilePath, fileContent);
            Console.WriteLine($"  ✓ Report saved to: {outputFilePath}");
        }

        public async Task<string> GetPrompt(string fileName, string sectorName = null!, string companyName = null!, string InputData = null!)
        {
            string promptFilePath = Path.Combine(_promptFolder, fileName);
            if (File.Exists(promptFilePath))
            {
                string prompt = await File.ReadAllTextAsync(promptFilePath);

                string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                string yesterdayDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                prompt = prompt.Replace("\r\n", "\n").Replace("\r", "\n").Replace("{todayDate}", todayDate).Replace("{yesterdayDate}", yesterdayDate);
                if (companyName != null)
                {
                    prompt = prompt.Replace("{companyName}", companyName);
                }
                if (sectorName != null)
                {
                    prompt = prompt.Replace("{sectorName}", sectorName);
                }
                if(InputData != null)
                {
                    prompt = prompt.Replace("[Insert Your JSON Here]", InputData);
                }

                return prompt;
            }
            else
            {
                Console.WriteLine($"  ✗ File not found: {promptFilePath}");
                return string.Empty;
            }
        }
    }
}
