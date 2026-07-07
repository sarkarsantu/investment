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

    /// <summary>
    /// Processes a single RSS feed, crawling all articles.
    /// </summary>
    public List<Link> ProcessFeed(RssFeedConfig feed, int maxArticlesPerFeed)
    {
        return FetchLinksFromFeed(feed.Url, maxArticlesPerFeed);
    }

    /// <summary>
    /// Fetches links from an RSS feed.
    /// </summary>
    private List<Link> FetchLinksFromFeed(string feedUrl, int maxArticles)
    {
        try
        {
            return _rssService.GetFeedItems(feedUrl, maxArticles);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Failed to fetch RSS: {feedUrl} - {ex.Message}");
            return new List<Link>();
        }
    }
}