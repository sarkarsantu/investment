using InvestmentResearch.Model;

namespace InvestmentResearch.Repository;

public interface ICompanyData
{
    CompanyResearch GetCompanyDataByIsin(string isin);
    CompanyResearch InsertCompanyDate(CompanyResearch companyResearch);
    CompanyResearch UpdateCompanyData(CompanyResearch companyResearch);
    int SaveCompanyDataFromOutsource(string NameFromOutsource);
    List<CompanyResearch> GetCompanyData(int top);
    CompanyResearch UpdateCompanyDataWithISIN(CompanyResearch companyResearch);
}
