using InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;

namespace InvestmentResearch.Service.Upstox
{
    public interface IUpstoxCompanyProfileService
    {
        Task<CompanyProfileResponse> GetCompanyProfileAsync(string isin);
    }
}
