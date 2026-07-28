namespace InvestmentResearch.Model
{
    public class AppSettings
    {
        public const string SectionName = "Investment";

        public string GeminiApiKey1 { get; set; } = string.Empty;
        public string GeminiApiKey2 { get; set; } = string.Empty;
        public string GeminiApiKey3 { get; set; } = string.Empty;
        public string GeminiApiKey4 { get; set; } = string.Empty;
        public string GeminiApiKey5 { get; set; } = string.Empty;
        public string GitHubToken { get; set; } = string.Empty;
        public string GitHubOwner { get; set; } = string.Empty;
        public string GitHubRepo { get; set; } = string.Empty;
        public string GitHubFolder { get; set; } = string.Empty;
        public string Sectors { get; set; } = string.Empty;
        public string ElectronicsManufacturingServices { get; set; } = string.Empty;
        public string RenewableEnergy { get; set; } = string.Empty;
        public string Defence { get; set; } = string.Empty;
        public string Battery { get; set; } = string.Empty;
        public List<Prompt> Prompts { get; set; } = new List<Prompt>();
        public List<RssFeedConfig> RssFeeds { get; set; } = new List<RssFeedConfig>();

        // SMTP / Email settings
        public string SmtpHost { get; set; } = string.Empty;
        public int SmtpPort { get; set; } = 587;
        public string SmtpUser { get; set; } = string.Empty;
        public string SmtpPass { get; set; } = string.Empty;
        public bool SmtpUseSsl { get; set; } = true;
        public string FromEmail { get; set; } = string.Empty;
        public string ExcludeDomain { get; set; }
    }
}
