using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IRssFeedDatabaseService
    {
        Task<List<RssFeedConfig>> GetAllFeedsAsync();
    }
}
