using InvestmentResearch.Model;

namespace InvestmentResearch.Helper;

public class HtmlGenerator
{
    public static string GenerateHtml(List<CompanyResult> results)
    {
        var html = new System.Text.StringBuilder();
        var sectors = results.Select(r => r.SectorName).Distinct().ToList();

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"UTF-8\">");
        html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine("<meta name=\"description\" content=\"Daily investment research report.\">");
        html.AppendLine("");
        html.AppendLine($"    <title>Investment Research</title>");
        html.AppendLine("    <style>");
        html.AppendLine("        body { font-family: Arial, sans-serif;margin: 20px;background-color: #f5f5f5;line-height: 1.6; }");
        html.AppendLine("main { max-width: 1000px;margin: auto; }");
        html.AppendLine("        h1 { color: #333;border-bottom: 2px solid #007bff;padding-bottom: 10px; }");
        html.AppendLine("        .company { background-color: white;margin: 15px 0;padding: 15px;border-radius: 5px;box-shadow: 0 2px 5px rgba(0,0,0,0.1); }");
        html.AppendLine("        .company h2 { color: #007bff;margin-top: 0; }");
        html.AppendLine("        .response { background-color: #f9f9f9;padding: 10px;border-left: 4px solid #007bff;margin-top: 10px; }");
        html.AppendLine("        .timestamp { color: #666; font-size: 12px;}");
        html.AppendLine("    </style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");

        html.AppendLine($"    <p><strong>Generated on:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");
        html.AppendLine("    <hr>");

        foreach (var sector in sectors)
        {
            html.AppendLine("<main>");
            html.AppendLine("<article>");
            html.AppendLine($"    <h1>{sector}</h1>");
            foreach (var result in results.Where(r => r.SectorName == sector))
            {
                html.AppendLine("    <div class=\"company\">");
                if (!string.IsNullOrEmpty(result.CompanyName) && !string.IsNullOrEmpty(result.Response))
                {
                    html.AppendLine($"        <h2>{result.CompanyName}</h2>");
                }
                html.AppendLine($"        <div class=\"response\">");
                html.AppendLine($"            {System.Web.HttpUtility.HtmlEncode(result.Response).Replace("\n", "<br>")}");
                html.AppendLine($"        </div>");
                html.AppendLine("    </div>");
            }
            html.AppendLine("</article>");
            html.AppendLine("</main>");
        }

        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    public static string GenerateRSSFeedHtml(List<Link> links, string sectorName)
    {
        // Extract domain from each link
        foreach (var link in links)
        {
            link.ExtractDomain();
        }

        var html = new System.Text.StringBuilder();

        // Get unique ranks sorted to assign alternating colors
        var uniqueRanks = links.Select(l => l.Rank).Distinct().OrderBy(r => r).ToList();
        var rankBackgroundMap = GetRankBackgroundMap(uniqueRanks);

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"UTF-8\">");
        html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine($"<title>RSS Feed - {System.Web.HttpUtility.HtmlEncode(sectorName)}</title>");
        html.AppendLine("<style>");
        html.AppendLine("    * { margin: 0; padding: 0; box-sizing: border-box; }");
        html.AppendLine("    body { font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; background-color: #f0f2f5; padding: 20px; }");
        html.AppendLine("    .container { max-width: 900px; margin: 0 auto; }");
        html.AppendLine("    h1 { color: #2c3e50; margin-bottom: 10px; text-align: center; }");
        html.AppendLine("    .info { text-align: center; color: #7f8c8d; margin-bottom: 20px; font-size: 14px; }");
        html.AppendLine("    .links-container { display: flex; flex-direction: column; gap: 15px; }");
        html.AppendLine("    .link-item { padding: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); transition: all 0.3s ease; cursor: pointer; border-left: 5px solid #3498db; }");
        html.AppendLine("    .link-item:hover { box-shadow: 0 4px 12px rgba(0,0,0,0.15); transform: translateY(-2px); }");
        html.AppendLine("    .link-item a { text-decoration: none; color: inherit; display: block; }");
        html.AppendLine("    .link-header { display: flex; justify-content: space-between; align-items: flex-start; margin-bottom: 10px; }");
        html.AppendLine("    .link-title { flex: 1; font-size: 16px; font-weight: 600; color: #2c3e50; word-break: break-word; }");
        html.AppendLine("    .link-meta { display: flex; gap: 10px; align-items: center; margin-left: 10px; }");
        html.AppendLine("    .rank-badge { background-color: #3498db; color: white; padding: 4px 12px; border-radius: 20px; font-size: 12px; font-weight: bold; white-space: nowrap; }");
        html.AppendLine("    .sl-no { color: #95a5a6; font-size: 12px; }");
        html.AppendLine("    .link-content { color: #555; font-size: 14px; line-height: 1.5; margin-bottom: 10px; max-height: 100px; overflow: hidden; text-overflow: ellipsis; }");
        html.AppendLine("    .link-footer { display: flex; justify-content: space-between; align-items: center; margin-top: 10px; font-size: 12px; color: #95a5a6; }");
        html.AppendLine("    .link-url { color: #3498db; text-decoration: underline; }");
        html.AppendLine("    .domain-badge { background-color: #e8f4f8; color: #2c3e50; padding: 4px 10px; border-radius: 4px; font-size: 11px; font-weight: 600; }");
        html.AppendLine("    .primary-bg { background-color: #ffffff; }");
        html.AppendLine("    .alternate-bg { background-color: #f9f9f9; }");

        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<div class=\"container\">");
        html.AppendLine($"<h1>📰 RSS Feed - {System.Web.HttpUtility.HtmlEncode(sectorName)}</h1>");
        html.AppendLine($"<div class=\"info\">Generated on {DateTime.Now:yyyy-MM-dd HH:mm:ss} | Total Links: {links.Count}</div>");
        html.AppendLine("<div class=\"links-container\">");

        var orderedLinks = links.OrderBy(l => l.Rank).ThenBy(l => l.SlNo).ToList();

        foreach (var link in orderedLinks)
        {
            var bgClass = rankBackgroundMap[link.Rank];

            html.AppendLine($"<div class=\"link-item {bgClass}\" onclick=\"window.open('{System.Web.HttpUtility.HtmlAttributeEncode(link.Href)}', '_blank')\">");
            html.AppendLine("<a href=\"#\" onclick=\"return false;\">");
            html.AppendLine("<div class=\"link-header\">");
            html.AppendLine($"<div class=\"link-title\">{System.Web.HttpUtility.HtmlEncode(link.Title)}</div>");
            html.AppendLine("<div class=\"link-meta\">");
            html.AppendLine($"<span class=\"rank-badge\">Rank: {link.Rank}</span>");
            html.AppendLine($"<span class=\"sl-no\">#{link.SlNo}</span>");
            html.AppendLine("</div>");
            html.AppendLine("</div>");

            if (!string.IsNullOrEmpty(link.Content))
            {
                html.AppendLine($"<div class=\"link-content\">{System.Web.HttpUtility.HtmlEncode(link.Content)}</div>");
            }

            html.AppendLine("<div class=\"link-footer\">");
            html.AppendLine($"<span class=\"domain-badge\">{System.Web.HttpUtility.HtmlEncode(link.Domain)}</span>");
            html.AppendLine($"<span class=\"link-url\" title=\"{System.Web.HttpUtility.HtmlAttributeEncode(link.Href)}\">Click to open →</span>");
            html.AppendLine("</div>");
            html.AppendLine("</a>");
            html.AppendLine("</div>");
        }

        html.AppendLine("</div>");
        html.AppendLine("</div>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    /// <summary>
    /// Creates a background color mapping for different ranks.
    /// Alternates between primary (white) and alternate (light gray) backgrounds.
    /// All links with the same rank will have the same background color.
    /// </summary>
    private static Dictionary<int, string> GetRankBackgroundMap(List<int> uniqueRanks)
    {
        var backgroundMap = new Dictionary<int, string>();
        bool usePrimaryBg = true;

        foreach (var rank in uniqueRanks)
        {
            backgroundMap[rank] = usePrimaryBg ? "primary-bg" : "alternate-bg";
            usePrimaryBg = !usePrimaryBg;
        }

        return backgroundMap;
    }
}
