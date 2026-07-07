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
    }
}
