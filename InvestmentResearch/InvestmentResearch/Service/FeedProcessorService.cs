using InvestmentResearch.Model;

namespace InvestmentResearch.Service;

/// <summary>
/// Service for processing RSS feeds and coordinating article crawling.
/// Responsibility: Fetch RSS feeds, extract links, and coordinate article processing
/// </summary>
public class FeedProcessorService : IFeedProcessorService
{
    private readonly IRssService _rssService;

    public FeedProcessorService(IRssService rssService)
    {
        _rssService = rssService;
    }
    public string GetUnknownDomains()
    {
        return _rssService.GetUnknownDomains();
    }

    /// <summary>
    /// Processes a single RSS feed, crawling all articles.
    /// </summary>
    public List<Link> ProcessFeed(RssFeedConfig feed, int maxArticlesPerFeed)
    {
        return FetchLinksFromFeed(feed.Url, feed.Name, maxArticlesPerFeed);
    }

    public List<Link> UpdateLinksBySLNo(List<Link> links)
    {
        return _rssService.UpdateLinksBySLNo(links);
    }

    /// <summary>
    /// Fetches links from an RSS feed.
    /// </summary>
    private List<Link> FetchLinksFromFeed(string feedUrl, string alert, int maxArticles)
    {
        try
        {
            return _rssService.GetFeedItems(feedUrl, alert, maxArticles);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch RSS: {feedUrl} - {ex.Message}");
            return new List<Link>();
        }
    }
}