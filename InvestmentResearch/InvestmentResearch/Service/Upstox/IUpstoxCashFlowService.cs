using InvestmentResearch.Model.Upstox.Fundamental.CashFlow;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxCashFlowService
    {
        Task<CashFlowResponse> GetCashFlowAsync(string isin);
    }
}
