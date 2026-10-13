using InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxShareHoldingsService
    {
        Task<ShareHoldingResponse> GetShareHoldingsAsync(string isin);
    }
}
