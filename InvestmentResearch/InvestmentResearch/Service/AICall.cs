using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using Microsoft.Extensions.Options;

namespace InvestmentResearch.Service
{
    public class AICall : IAICall
    {
        private readonly IDailyEntry _dailyEntry;
        private readonly IFileHelper _fileHelper;
        private readonly IOptions<AppSettings> _appConfig;
        private List<Company> _allCompanies = new List<Company>();
        private List<Sector> _allSectors = new List<Sector>();

        public AICall(IDailyEntry dailyEntry, IFileHelper fileHelper, IOptions<AppSettings> appSettings)
        {
            _dailyEntry = dailyEntry;
            _fileHelper = fileHelper;
            _appConfig = appSettings ?? throw new ArgumentNullException(nameof(appSettings));
        }
        
        private List<Sector> AllSectors
        {
            get
            {
                if (_allSectors == null || _allSectors.Count == 0)
                {
                    _allSectors = _dailyEntry.GetAllSectors();
                }
                return _allSectors;
            }
        }

        private List<Company> AllCompanies
        {
            get
            {
                if (_allCompanies == null || _allCompanies.Count == 0)
                {
                    _allCompanies = _dailyEntry.GetDailyCompanies();
                }
                return _allCompanies;
            }
        }

        public async Task<List<CompanyResult>> GenerateContent(Prompt prompt)
        {
            var geminiService1 = new GeminiService(_appConfig);

            var companyResults = new List<CompanyResult>();
            if (prompt.CompanyWise)
            {
                foreach (var company in AllCompanies)
                {
                    Console.WriteLine("Fetching news - " + company.Name);
                    string companyPrompt = await _fileHelper.GetPrompt(prompt.FileName, company.SectorName, company.Name);
                    var result = await geminiService1.GenerateContent(companyPrompt);
                    if(result != null && result != new CompanyResult() && !string.IsNullOrEmpty(result.Response))
                    {
                        result.SectorName = company.SectorName;
                        result.CompanyName = company.Name;
                        companyResults.Add(result);
                    }
                }
            }
            else if (prompt.SectorWise)
            {
                foreach (var sector in AllSectors)
                {
                    string promptContent = await _fileHelper.GetPrompt(prompt.FileName, sector.Name);
                    var result = await geminiService1.GenerateContent(promptContent);
                    if (result != null && result != new CompanyResult())
                    {
                        result.SectorName = sector.Name;
                        companyResults.Add(result);
                    }
                }
            }
            else 
            {
                string promptContent = await _fileHelper.GetPrompt(prompt.FileName);
                var result = await geminiService1.GenerateContent(promptContent);
                if (result != null && result != new CompanyResult())
                {
                    companyResults.Add(result);
                }
            }

            return companyResults;
        }
    }
}
