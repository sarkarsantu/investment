using InvestmentResearch.Repository;
using InvestmentResearch.Service.Upstox;

namespace InvestmentResearch.Service
{
    public class LoadCompaniesService : ILoadCompaniesService
    {
        private readonly IFileHelper _fileHelper;
        private readonly ICompanyData _companyData;
        private readonly IInstrumentSearchService _instrumentSearchService;
        private readonly ICompanyResearchService _companyResearchService;

        public LoadCompaniesService(
            IFileHelper fileHelper, 
            ICompanyData companyData,
            IInstrumentSearchService instrumentSearchService,
            ICompanyResearchService companyResearchService)
        {
            _fileHelper = fileHelper;
            _companyData = companyData;
            _instrumentSearchService = instrumentSearchService;
            _companyResearchService = companyResearchService;
        }

        public void LoadBSECompaniesFromDhan()
        {
            List<string> bseCompanies = new List<string>();
            bseCompanies = LoadCompaniesFromDhan("Stocks/BSE");
            SaveCompaniesToDatabase(bseCompanies);
        }

        public void LoadNSECompaniesFromDhan()
        {
            List<string> nseCompanies = new List<string>();
            nseCompanies = LoadCompaniesFromDhan("Stocks/NSE");
            SaveCompaniesToDatabase(nseCompanies);
        }

        private List<string> LoadCompaniesFromDhan(string path)
        {
            List<string> companies = new List<string>();
            string[] files = Directory.GetFiles(path);
            foreach (string file in files)
            {
                var companiesByAlphabet = _fileHelper.GetAllCompaniesFromDhan(file);
                if (companiesByAlphabet != null && companiesByAlphabet.Count > 0)
                {
                    companies.AddRange(companiesByAlphabet);
                }
            }
            return companies;
        }

        private void SaveCompaniesToDatabase(List<string> companies)
        {
            foreach (var company in companies)
            {
                _companyData.SaveCompanyDataFromOutsource(company);
            }
        }

        public async Task UpdateCompaniesFromDatabase(int top)
        {
            var companies = _companyData.GetCompanyData(top);
            foreach (var company in companies)
            {
                var instrumentSearchResponse = await  _instrumentSearchService.GetCompanyISIN(company.NameFromOutsource ?? string.Empty);
                if (instrumentSearchResponse != null && instrumentSearchResponse.Status?.ToLower() == "success".ToLower())
                {
                    var companyResearch = new Model.CompanyResearch
                    {
                        ISIN = instrumentSearchResponse.Data?.FirstOrDefault()?.Isin,
                        CompanyName = instrumentSearchResponse.Data?.FirstOrDefault()?.Name,
                        ModifyDate = DateTime.Now,
                        NoOfAttempts = 1,
                        NameFromOutsource = company.NameFromOutsource ?? string.Empty
                    };
                    if(companyResearch == null || string.IsNullOrEmpty(companyResearch.ISIN))
                    {
                        Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## ISIN not found for company: {company.NameFromOutsource}");
                        continue;
                    }
                    _companyData.UpdateCompanyDataWithISIN(companyResearch);
                    Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {company.NameFromOutsource} - ISIN: {companyResearch.ISIN}");
                    await _companyResearchService.RunReachModel(companyResearch.ISIN, companyResearch.CompanyName ?? string.Empty,string.Empty);
                }
                // Delays execution for 1 minute without blocking the thread
                await Task.Delay(TimeSpan.FromMinutes(1));
            }
        }
    }
}
