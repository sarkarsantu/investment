using InvestmentResearch.Model;
using Microsoft.Extensions.Options;
using System.Reflection;

namespace InvestmentResearch.Repository
{
    public class DailyEntry : IDailyEntry
    {
        private readonly AppSettings _appSettings;

        public DailyEntry(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public List<Company> GetDailyCompanies()
        {
            var companies = new List<Company>();

            var sectors = GetAllSectors();
            Console.WriteLine($"Found {sectors.Count} sectors. Starting research...\n");

            foreach (var sector in sectors)
            {
                Console.WriteLine($"Processing sector: {sector.Name}");
                var companiesBySectorName = GetCompaniesBySectorName(sector.Name);
                if (companiesBySectorName != null && companiesBySectorName.Count > 0)
                {
                    companies.AddRange(companiesBySectorName);
                }
            }

            return companies;
        }

        public List<Sector> GetAllSectors()
        {
            var sectors = new List<Sector>();
            int i = 0;
            foreach (var sector in _appSettings.Sectors.Split(","))
            {
                sectors.Add(new Sector { Id = i++, Name = sector });
            }

            return sectors;
        }

        private List<Company> GetCompaniesBySectorName(string sectorName)
        {
            var companies = new List<Company>();

            // 1. Specify the string name of the property you want to fetch
            string propertyName = sectorName.Replace(" ","");

            // 2. Get the Type of the object
            Type type = this._appSettings.GetType();

            // 3. Retrieve the PropertyInfo metadata matching the string name
            PropertyInfo propInfo = type.GetProperty(propertyName) ?? throw new Exception($"{propertyName} is not found in appConfig");

            // 5. Check for null to ensure the property actually exists
            if (propInfo != null)
            {
                // 6. Extract the value from the specific object instance
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
                string companyNames = (string)propInfo.GetValue(_appSettings) ?? throw new Exception($"{propertyName} is not found in appConfig");
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.

                int i = 0;
                foreach (var companyName in companyNames.Split(','))
                {
                    var company = new Company()
                    {
                        Id = i++,
                        Name = companyName,
                        SectorName = sectorName
                    };
                    company.Prompt.Replace("{companyName}", companyName);
                    companies.Add(company);
                }
            }
            else
            {
                throw new Exception($"{propertyName} is not found in appConfig");
            }

            return companies;
        }
    }
}
