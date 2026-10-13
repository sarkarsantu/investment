namespace InvestmentResearch.Model.Upstox.IPO
{
    public class IPOData
    {
        public string Id { get; set; } = string.Empty;
        public string Symbol { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;
        public string Isin { get; set; } = string.Empty;
        public string IssueType { get; set; } = string.Empty;
        public int IssueSize { get; set; }
        public string Industry { get; set; } = string.Empty;
        public int MinimumPrice { get; set; }
        public int MaximumPrice { get; set; }
        public string BiddingStartDate { get; set; } = string.Empty;
        public string BiddingEndDate { get; set; } = string.Empty;
        public string TotalSubscription { get; set; } = string.Empty;
    }
}
