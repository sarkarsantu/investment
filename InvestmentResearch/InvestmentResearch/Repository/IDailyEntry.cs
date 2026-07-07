using InvestmentResearch.Model;

namespace InvestmentResearch.Repository
{
    public interface IDailyEntry
    {
        List<Company> GetDailyCompanies();
        List<Sector> GetAllSectors();
    }
}
