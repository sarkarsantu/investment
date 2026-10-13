using InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;

namespace InvestmentResearch.Service.Upstox
{
    public interface IInstrumentSearchService
    {
        Task<InstrumentSearchResponse> GetCompanyISIN(string nameFromOutsource);
    }
}
