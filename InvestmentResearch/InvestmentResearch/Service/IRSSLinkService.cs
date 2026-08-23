using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IRSSLinkService
    {
        List<Link> UpdateLinksBySLNo(List<Link> links);
        (string,string) ExtractDomain(string href);
    }
}
