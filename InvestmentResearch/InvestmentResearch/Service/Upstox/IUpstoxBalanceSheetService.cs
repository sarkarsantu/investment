using InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxBalanceSheetService
    {
        Task<BalanceSheetResponse> GetBalanceSheetAsync(string isin);
    }
}
