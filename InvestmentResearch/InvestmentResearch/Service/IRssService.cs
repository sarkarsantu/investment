namespace InvestmentResearch.Service;

using InvestmentResearch.Model;

public interface IRssService
{
    List<Link> GetFeedItems(string rssUrl, string theme, int maxItems);
    List<Link> UpdateLinksBySLNo(List<Link> links);
}
