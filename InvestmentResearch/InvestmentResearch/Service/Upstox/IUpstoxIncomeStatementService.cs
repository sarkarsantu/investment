using InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxIncomeStatementService
    {
        Task<IncomeStatementResponse> GetIncomeStatementAsync(string isin);
    }
}
