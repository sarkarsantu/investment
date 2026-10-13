using InvestmentResearch.Model;

namespace InvestmentResearch.Service;

public interface ICompanyResearchService
{
    Task<CompanyResearch?> RunReachModel(string Isin, string CompanyName, string Symbol);
}
