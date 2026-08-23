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
        var html = new System.Text.StringBuilder();
        var strikeTagStart = string.Empty;
        var strikeTagEnd = string.Empty;

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
        html.AppendLine("    .container { margin: 0 auto; }");
        html.AppendLine("    h1 { color: #2c3e50; margin-bottom: 10px; text-align: center; }");
        html.AppendLine("    .info { text-align: center; color: #7f8c8d; margin-bottom: 20px; font-size: 14px; }");
        html.AppendLine("    .links-container { display: inline; flex-direction: column; gap: 15px; }");
        html.AppendLine("    .link-item { transition: all 0.3s ease; cursor: pointer; flex-direction: column; }");
        html.AppendLine("    .link-item:hover { box-shadow: 0 4px 12px rgba(0,0,0,0.15); transform: translateY(-2px); }");
        html.AppendLine("    .link-item a { text-decoration: none; color: #427fed; display: block; }");
        html.AppendLine("    .link-header { display: flex; flex-direction: column; justify-content: space-between; }");
        html.AppendLine("    .link-title { flex: 1; font-size: 16px; font-weight: 600; color: #427fed; word-break: break-word; }");
        html.AppendLine("    .link-meta { display: flex; gap: 10px; align-items: center; margin-left: 10px; }");
        html.AppendLine("    .rank-badge { background-color: #3498db; color: white; padding: 4px 12px; border-radius: 20px; font-size: 12px; font-weight: bold; white-space: nowrap; }");
        html.AppendLine("    .sl-no { color: #95a5a6; font-size: 12px; }");
        html.AppendLine("    .link-content { color: #555; font-size: 14px; line-height: 1.5; max-height: 100px; overflow: hidden; text-overflow: ellipsis;padding-bottom: 10px; }");
        html.AppendLine("    .link-footer { display: flex; justify-content: space-between; align-items: center; font-size: 12px; color: #95a5a6; margin-top: auto; }");
        html.AppendLine("    .link-url { color: #3498db; text-decoration: underline; }");
        html.AppendLine("    .domain-badge { background-color: #e8f4f8; color: #2c3e50; padding: 4px 10px; border-radius: 4px; font-size: 11px; font-weight: 600; }");
        html.AppendLine("    .primary-bg { background-color: #ffffff; border-bottom: 1px solid; padding-top: 7px;}");
        html.AppendLine("    .alternate-bg { background-color: #f0f0f0; border-bottom: 1px solid; padding-top: 7px;}");

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
            if (!link.IsRelevant)
            {
                strikeTagStart = "<strike>";
                strikeTagEnd = "</strike>";
            }
            else
            {
                strikeTagStart = string.Empty;
                strikeTagEnd = string.Empty;
            }

            html.AppendLine($"<div class=\"link-item {bgClass}\")\">");
            html.AppendLine("<div class=\"link-header\">");
            html.AppendLine($"<div class=\"link-title\"><a href=\"{System.Web.HttpUtility.HtmlAttributeEncode(link.Href)}\" '_blank'>#Rank - {link.Rank}. {strikeTagStart}{System.Web.HttpUtility.HtmlEncode(link.Title)}{strikeTagEnd}<a></div>");
            html.AppendLine("</div>");
            html.AppendLine("<div class=\"link-footer\">");
            html.AppendLine($"<span {(link.SlNo <= 5 ? "style=\"color:red\"" : "")} class=\"domain-badge\">{System.Web.HttpUtility.HtmlEncode(link.Domain)} - </span>");
            html.AppendLine($"<span {(IsHotTheme(link.Theme) ? "style=\"color:red\"" : "")}  class=\"domain-badge\">{System.Web.HttpUtility.HtmlEncode(link.Theme)}</span>");
            html.AppendLine("</div>");
            if (!string.IsNullOrEmpty(link.Content))
            {
                html.AppendLine($"<div {(IsHotContent(link.Theme) ? "style=\"color:red\"" : "")} class=\"link-content\">{System.Web.HttpUtility.HtmlEncode(link.Content)}</div>");
            }

            html.AppendLine("</div>");
        }

        html.AppendLine("</div>");
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }

    public static bool IsHotTheme(string? theme)
    {
        theme = theme?.Trim()?.Replace("\"", "") ?? string.Empty;

        List<string> hotTheme = new();
        hotTheme.Add("radar AI processor India");
        hotTheme.Add("ISR AI computing India");
        hotTheme.Add("industrial vision AI chip");
        hotTheme.Add("Indian fabless semiconductor AI chip");
        hotTheme.Add("Indian edge AI SoC tape-out");
        hotTheme.Add("Indian defence edge AI chip");
        hotTheme.Add("Indian defence AI chip");
        hotTheme.Add("sodium-ion battery India");
        hotTheme.Add("sodium-ion AND (India OR Indian)");
        hotTheme.Add("sodium-ion AND (IIT OR JNCASR OR IISc)");
        hotTheme.Add("sodium battery research India");

        foreach (var hot in hotTheme)
        {
            if (theme.Contains(hot, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    public static bool IsHotContent(string? content)
    {
        content = content?.Trim()?.Replace("\"", "") ?? string.Empty;

        List<string> hotContent = new();
        hotContent.Add("IIT");
        hotContent.Add("isro");
        hotContent.Add("research");
        hotContent.Add("drdo");
        hotContent.Add("scientist");

        foreach (var hot in hotContent)
        {
            if (content.Contains(hot, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
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

    public static string GenerateSummarySectorHtml(List<TrackBySector> trackBySectors, string UnknownDomains, List<string> logs)
    {
        var html = new System.Text.StringBuilder();

        html.AppendLine("<!DOCTYPE html>");
        html.AppendLine("<html>");
        html.AppendLine("<head>");
        html.AppendLine("<meta charset=\"UTF-8\">");
        html.AppendLine("<meta name=\"viewport\" content=\"width=device-width, initial-scale=1.0\">");
        html.AppendLine("<title>Investment Research Summary</title>");
        html.AppendLine("<style>");
        html.AppendLine("    body { font-family: Arial, sans-serif; margin: 20px; background-color: #f5f5f5; line-height: 1.6; }");
        html.AppendLine("    .container { max-width: 1000px; margin: auto; }");
        html.AppendLine("    h1 { color: #333; border-bottom: 2px solid #007bff; padding-bottom: 10px; }");
        html.AppendLine("    .sector { background-color: white; margin: 15px 0; padding: 15px; border-radius: 5px; box-shadow: 0 2px 5px rgba(0,0,0,0.1); }");
        html.AppendLine("    .sector h2 { color: #007bff; margin-top: 0; }");
        html.AppendLine("    .sector-stats { display: grid; grid-template-columns: repeat(auto-fit, minmax(150px, 1fr)); gap: 10px; margin: 10px 0; }");
        html.AppendLine("    .stat { background-color: #f9f9f9; padding: 10px; border-left: 4px solid #007bff; }");
        html.AppendLine("    .stat-label { font-weight: bold; color: #555; font-size: 12px; }");
        html.AppendLine("    .stat-value { font-size: 18px; color: #007bff; font-weight: bold; }");
        html.AppendLine("    .google-alert { background-color: #fafafa; padding: 10px; margin: 8px 0; border-left: 3px solid #17a2b8; }");
        html.AppendLine("    .google-alert-name { font-weight: bold; color: #333; }");
        html.AppendLine("    .alert-stats { font-size: 12px; color: #666; margin-top: 5px; }");
        html.AppendLine("    .timestamp { text-align: center; color: #7f8c8d; margin-top: 20px; font-size: 12px; border-top: 1px solid #ddd; padding-top: 10px; }");
        html.AppendLine("</style>");
        html.AppendLine("</head>");
        html.AppendLine("<body>");
        html.AppendLine("<div class=\"container\">");
        html.AppendLine($"<h1>Investment Research Summary</h1>");
        html.AppendLine($"<p><strong>Generated on:</strong> {DateTime.Now:yyyy-MM-dd HH:mm:ss}</p>");

        int totalInitialLinks = 0;
        int totalFinalLinks = 0;
        int totalMaxRank = 0;

        foreach (var sector in trackBySectors)
        {
            totalInitialLinks += sector.LinkCount;
            totalFinalLinks += sector.FinalLinkCount;
            totalMaxRank += sector.MaxRank;

            html.AppendLine($"<div class=\"sector\">");
            html.AppendLine($"    <h2>{System.Web.HttpUtility.HtmlEncode(sector.Sector)}</h2>");
            html.AppendLine($"    <div class=\"sector-stats\">");
            html.AppendLine($"        <div class=\"stat\">");
            html.AppendLine($"            <div class=\"stat-label\">Initial Links</div>");
            html.AppendLine($"            <div class=\"stat-value\">{sector.LinkCount}</div>");
            html.AppendLine($"        </div>");
            html.AppendLine($"        <div class=\"stat\">");
            html.AppendLine($"            <div class=\"stat-label\">Final Links</div>");
            html.AppendLine($"            <div class=\"stat-value\">{sector.FinalLinkCount}</div>");
            html.AppendLine($"        </div>");
            html.AppendLine($"        <div class=\"stat\">");
            html.AppendLine($"            <div class=\"stat-label\">Max Rank</div>");
            html.AppendLine($"            <div class=\"stat-value\">{sector.MaxRank}</div>");
            html.AppendLine($"        </div>");
            html.AppendLine($"    </div>");

            if (sector.GoogleAlerts != null && sector.GoogleAlerts.Count > 0)
            {
                html.AppendLine($"    <div style=\"margin-top: 15px;\">");
                html.AppendLine($"        <strong>Google Alerts ({sector.GoogleAlerts.Count}):</strong>");
                foreach (var alert in sector.GoogleAlerts)
                {
                    html.AppendLine($"        <div class=\"google-alert\">");
                    html.AppendLine($"            <div class=\"google-alert-name\">{System.Web.HttpUtility.HtmlEncode(alert.GoogleAlertName)}</div>");
                    html.AppendLine($"            <div class=\"alert-stats\">Initial: {alert.LinkCount} | Final: {alert.FinalLinkCount}</div>");
                    html.AppendLine($"        </div>");
                }
                html.AppendLine($"    </div>");
            }

            html.AppendLine($"</div>");
        }

        html.AppendLine($"<div class=\"sector\" style=\"background-color: #e8f4f8;\">");
        html.AppendLine($"    <h2 style=\"color: #17a2b8;\">Total Summary</h2>");
        html.AppendLine($"    <div class=\"sector-stats\">");
        html.AppendLine($"        <div class=\"stat\">");
        html.AppendLine($"            <div class=\"stat-label\">Total Sectors</div>");
        html.AppendLine($"            <div class=\"stat-value\">{trackBySectors.Count}</div>");
        html.AppendLine($"        </div>");
        html.AppendLine($"        <div class=\"stat\">");
        html.AppendLine($"            <div class=\"stat-label\">Total Initial Links</div>");
        html.AppendLine($"            <div class=\"stat-value\">{totalInitialLinks}</div>");
        html.AppendLine($"        </div>");
        html.AppendLine($"        <div class=\"stat\">");
        html.AppendLine($"            <div class=\"stat-label\">Total Final Links</div>");
        html.AppendLine($"            <div class=\"stat-value\">{totalFinalLinks}</div>");
        html.AppendLine($"        </div>");
        html.AppendLine($"        <div class=\"stat\">");
        html.AppendLine($"            <div class=\"stat-label\">Total Max Rank</div>");
        html.AppendLine($"            <div class=\"stat-value\">{totalMaxRank}</div>");
        html.AppendLine($"        </div>");
        html.AppendLine($"    </div>");
        html.AppendLine($"</div>");

        html.AppendLine($"<div class=\"sector\" style=\"background-color: #e8f4f8;\">");
        html.AppendLine($"    <h2 style=\"color: #17a2b8;\">Logs</h2>");
        html.AppendLine($"    <div class=\"sector-stats\">");
        foreach (var log in logs)
        {
            html.AppendLine($"        <div class=\"stat\">");
            html.AppendLine($"            <div class=\"stat-value\">{log}</div>");
            html.AppendLine($"        </div>");
        }
        html.AppendLine($"</div>");
        html.AppendLine($"</div>");
        html.AppendLine($"<div class=\"timestamp\">This is an automated email from Investment Research System</div>");
        if (!string.IsNullOrEmpty(UnknownDomains))
        {
            html.AppendLine($"<div class=\"timestamp\" style=\"color: red;\">⚠️ Warning: Some links have unknown domains. Please check the details.{UnknownDomains}</div>");
        }
        html.AppendLine("</body>");
        html.AppendLine("</html>");

        return html.ToString();
    }
}
