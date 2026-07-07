using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public interface IFeedProcessorService
    {
        List<Link> ProcessFeed(RssFeedConfig feed, int maxArticlesPerFeed);
    }
}
