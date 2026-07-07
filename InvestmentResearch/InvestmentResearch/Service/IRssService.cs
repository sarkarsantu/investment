namespace InvestmentResearch.Service;

using InvestmentResearch.Model;

public interface IRssService
{
    List<string> GetLinks(string rssUrl, int maxItems);
    List<Link> GetFeedItems(string rssUrl, int maxItems);
}
