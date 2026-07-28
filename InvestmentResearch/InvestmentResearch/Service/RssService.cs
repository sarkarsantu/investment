using InvestmentResearch.Helper;
using InvestmentResearch.Model;
using Microsoft.Extensions.Options;
using System.Web;
using System.Xml;

namespace InvestmentResearch.Service;

public class RssService : IRssService
{
    private readonly AppSettings _appSettings;
    private readonly IRSSLinkService _rSSLinkService;

    public RssService(IRSSLinkService rSSLinkService,IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _rSSLinkService = rSSLinkService;
    }

    public List<Link> GetFeedItems(string rssUrl, string theme, int maxItems) 
    {
        var links = new List<Link>();

        using var client = HttpClientHelper.CreateBrowserLikeClient();

        // HttpClientHandler has AutomaticDecompression enabled, so this gets decompressed automatically
        var xml = client.GetStringAsync(rssUrl).Result;

        var doc = new XmlDocument();
        doc.LoadXml(xml);

        // Try RSS format first (//item)
        var nodes = doc.SelectNodes("//item");

        if (nodes.Count > 0)
        {
            // RSS format
            foreach (XmlNode node in nodes)
            {
                var link = ParseRssItem(node);
                link.Domain = _rSSLinkService.ExtractDomain(link?.Href ?? string.Empty);
                if (link != null 
                    && IsPublishedTodayOrYesterday(link.Published) 
                    && _appSettings.ExcludeDomain.IndexOf(link.Domain, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    link.Domain = this._rSSLinkService.ExtractDomain(link.Href);
                    link.Theme = theme;
                    links.Add(link);
                    if (links.Count >= maxItems) break;
                }
            }
        }
        else
        {
            // Try Atom format (//entry)
            var nsManager = new XmlNamespaceManager(doc.NameTable);
            nsManager.AddNamespace("atom", "http://www.w3.org/2005/Atom");

            nodes = doc.SelectNodes("//atom:entry", nsManager);

            foreach (XmlNode node in nodes)
            {
                var link = ParseAtomEntry(node, nsManager);
                link.Domain = _rSSLinkService.ExtractDomain(link?.Href ?? string.Empty);
                if (link != null 
                    && IsPublishedTodayOrYesterday(link.Published) 
                    && _appSettings.ExcludeDomain.IndexOf(link.Domain, StringComparison.OrdinalIgnoreCase) == -1)
                {
                    link.Domain = this._rSSLinkService.ExtractDomain(link.Href);
                    link.Theme = theme;
                    links.Add(link);
                    if (links.Count >= maxItems) break;
                }
            }
        }

        return links;
    }

    private Link? ParseRssItem(XmlNode itemNode)
    {
        try
        {
            var id = itemNode.SelectSingleNode("guid")?.InnerText ?? Guid.NewGuid().ToString();
            var title = itemNode.SelectSingleNode("title")?.InnerText ?? string.Empty;
            var href = itemNode.SelectSingleNode("link")?.InnerText ?? string.Empty;
            var published = itemNode.SelectSingleNode("pubDate")?.InnerText;
            var content = itemNode.SelectSingleNode("description")?.InnerText ?? string.Empty;

            if (string.IsNullOrWhiteSpace(href))
                return null;

            href = ExtractActualUrl(href);

            return new Link
            {
                Id = id,
                Title = StripHtmlTags(title),
                Href = href,
                Published = NormalizeDate(published),
                Content = StripHtmlTags(content)
            };
        }
        catch
        {
            return null;
        }
    }

    private Link? ParseAtomEntry(XmlNode entryNode, XmlNamespaceManager nsManager)
    {
        try
        {
            var id = entryNode.SelectSingleNode("atom:id", nsManager)?.InnerText ?? Guid.NewGuid().ToString();
            var title = entryNode.SelectSingleNode("atom:title", nsManager)?.InnerText ?? string.Empty;
            
            // Get the link href - handle Google Alerts format which doesn't always have rel attribute
            var href = string.Empty;
            
            // Try to get all link nodes
            var linkNodes = entryNode.SelectNodes("atom:link", nsManager);
            
            if (linkNodes.Count > 0)
            {
                // Prefer link with rel='alternate', otherwise take the first one
                XmlNode linkNode = null;
                
                foreach (XmlNode node in linkNodes)
                {
                    var rel = node.Attributes?["rel"]?.Value;
                    if (rel == "alternate")
                    {
                        linkNode = node;
                        break;
                    }
                }
                
                // If no alternate link found, take the first link node
                if (linkNode == null && linkNodes.Count > 0)
                {
                    linkNode = linkNodes[0];
                }
                
                href = linkNode?.Attributes?["href"]?.Value ?? string.Empty;
            }
            
            var published = entryNode.SelectSingleNode("atom:published", nsManager)?.InnerText;
            var content = entryNode.SelectSingleNode("atom:content", nsManager)?.InnerText ?? string.Empty;

            if (string.IsNullOrWhiteSpace(href))
                return null;

            href = ExtractActualUrl(href);

            return new Link
            {
                Id = id,
                Title = StripHtmlTags(title),
                Href = href,
                Published = NormalizeDate(published),
                Content = StripHtmlTags(content)
            };
        }
        catch
        {
            return null;
        }
    }

    private string ExtractActualUrl(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return url;

        // Handle Google Alerts redirect URLs
        if (url.Contains("google.com/url"))
        {
            try
            {
                var uri = new Uri(url);
                var queryParams = HttpUtility.ParseQueryString(uri.Query);
                var actualUrl = queryParams["url"];

                if (!string.IsNullOrWhiteSpace(actualUrl))
                    return actualUrl;
            }
            catch
            {
                // If parsing fails, return original URL
            }
        }

        return url;
    }

    private string NormalizeDate(string? dateString)
    {
        try
        {
            DateTime parsedDate;
            if (!string.IsNullOrWhiteSpace(dateString) && DateTime.TryParse(dateString, out parsedDate))
            {
                return parsedDate.ToString("yyyy-MM-dd");
            }
            else
            {
                return DateTime.Today.ToString("yyyy-MM-dd");
            }
        }
        catch { }

        return DateTime.Today.ToString("yyyy-MM-dd");
    }

    private string StripHtmlTags(string html)
    {
        if (string.IsNullOrWhiteSpace(html))
            return string.Empty;

        // First, handle JSON-escaped Unicode sequences (e.g., \u003Cb becomes <b>)
        html = System.Text.RegularExpressions.Regex.Replace(html, @"\\u([0-9A-Fa-f]{4})", m =>
        {
            return ((char)int.Parse(m.Groups[1].Value, System.Globalization.NumberStyles.HexNumber)).ToString();
        });

        // Remove HTML tags
        var tagRegex = new System.Text.RegularExpressions.Regex(@"<[^>]+>");
        html = tagRegex.Replace(html, string.Empty);

        // Remove HTML entity encoding
        html = System.Net.WebUtility.HtmlDecode(html);

        // Remove any remaining HTML entities that might not be decoded
        var entityRegex = new System.Text.RegularExpressions.Regex(@"&#\d+;|&[a-z]+;");
        html = entityRegex.Replace(html, string.Empty);

        // Remove non-breaking spaces and other special whitespace entities
        html = html.Replace("\u00A0", " ").Replace("\u0027", "'").Replace("\u200B", "");

        // Collapse multiple spaces into one
        html = System.Text.RegularExpressions.Regex.Replace(html, @"\s+", " ");

        return html.Trim();
    }

    private bool IsPublishedTodayOrYesterday(string publishedDate)
    {
        try
        {
            if (DateTime.TryParse(publishedDate, out var date))
            {
                var today = DateTime.Today;
                var yesterday = today.AddDays(-1);
                var dateOnly = date.Date;

                return dateOnly == today || dateOnly == yesterday;
            }
        }
        catch { }

        return false;
    }

    public List<Link> UpdateLinksBySLNo(List<Link> links)
    {
        return this._rSSLinkService.UpdateLinksBySLNo(links);
    }
}
