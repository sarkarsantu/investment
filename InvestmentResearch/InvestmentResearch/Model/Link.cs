namespace InvestmentResearch.Model;

public class Link
{
    public int SlNo { get; set; } = 0;
    public int Rank { get; set; } = 0;
    public required string Id { get; set; }
    public required string Title { get; set; }
    public required string Href { get; set; }
    public string Published { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    public string Domain { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRelevant { get; set; } = true;
    public bool IsDuplicate { get; set; } = false;

    /// <summary>
    /// Extracts the domain name from the Href property.
    /// Examples: "https://timesofindia.com/..." → "Times of India"
    /// </summary>
    public void ExtractDomain()
    {
        if (string.IsNullOrEmpty(Href))
        {
            Domain = "Unknown";
            return;
        }

        try
        {
            var uri = new Uri(Href);
            var hostParts = uri.Host.Split('.');

            // Get the domain name (second-to-last part before TLD)
            if (hostParts.Length >= 2)
            {
                string domainName = hostParts[^2]; // Get second-to-last part

                // Convert to title case and add spaces for compound names
                Domain = ConvertDomainToReadable(domainName);
            }
            else
            {
                Domain = uri.Host;
            }
        }
        catch
        {
            Domain = "Unknown";
        }
    }

    private string ConvertDomainToReadable(string domainName)
    {
        // Handle common cases
        var domainMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "timesofindia", "Times of India" },
            { "livemint", "Livemint" },
            { "moneycontrol", "Moneycontrol" },
            { "economictimes", "Economic Times" },
            { "thehindubusinessline", "The Hindu Business Line" },
            { "financialexpress", "Financial Express" },
            { "business-standard", "Business Standard" },
            { "bseindia", "BSE India" },
            { "nseindia", "NSE India" },
            { "bloomberg", "Bloomberg" },
            { "reuters", "Reuters" },
            { "cnbc", "CNBC" },
            { "cnbctv18", "CNBC-TV18" },
            { "indianexpress", "Indian Express" }
        };

        if (domainMap.TryGetValue(domainName, out var readableName))
        {
            return readableName;
        }

        // Default: convert camelCase and add spaces
        return System.Text.RegularExpressions.Regex.Replace(
            domainName,
            "(?<!^)(?=[A-Z])",
            " "
        );
    }
}
