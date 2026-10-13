using InvestmentResearch.Model.Upstox.Fundamental;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxFundamentalsService
    {
        Task<UpstoxFundamentalsData> GetAllFundamentalsAsync(string isin, string symbol, string companyName);
    }
}
