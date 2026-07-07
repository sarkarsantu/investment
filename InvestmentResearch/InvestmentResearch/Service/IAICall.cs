using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IAICall
    {
        Task<List<CompanyResult>> GenerateContent(Prompt prompt);
    }
}
