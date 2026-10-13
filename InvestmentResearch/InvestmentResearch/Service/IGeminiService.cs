using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IGeminiService
    {
        Task<CompanyResult> GenerateContent(string prompt);
        Task<string> GenerateContentAsync(string systemInstruction, string userData);
    }
}
