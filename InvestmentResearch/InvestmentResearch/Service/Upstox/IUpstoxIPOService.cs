using InvestmentResearch.Model.Upstox.IPO;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxIPOService
    {
        Task<List<IPOData>> GetOpenIPOsAsync();
    }
}
