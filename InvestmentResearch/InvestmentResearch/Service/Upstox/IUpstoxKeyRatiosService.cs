using InvestmentResearch.Model.Upstox.Fundamental.KeyRatios;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxKeyRatiosService
    {
        Task<KeyRatiosResponse> GetKeyRatiosAsync(string isin);
    }
}
