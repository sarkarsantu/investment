using InvestmentResearch.Model.Upstox.Fundamental.CorporateActions;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxCorporateActionsService
    {
        Task<CorporateActionsResponse> GetCorporateActionsAsync(string isin);
    }
}
