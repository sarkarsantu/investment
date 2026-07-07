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
}
