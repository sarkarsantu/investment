using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IRssFeedDatabaseService
    {
        Task SeedFromAppConfigAsync();
        Task<List<RssFeedConfig>> GetAllFeedsAsync();
    }
}
