namespace InvestmentResearch.Model.Upstox.IPO
{
    public class UpstoxIPOResponse
    {
        public string Status { get; set; } = string.Empty;
        public List<IPOData> Data { get; set; } = new List<IPOData>();
        public MetaData MetaData { get; set; } = new MetaData();
    }

    public class MetaData
    {
        public PageInfo Page { get; set; } = new PageInfo();
    }

    public class PageInfo
    {
        public int PageNumber { get; set; }
        public int TotalPages { get; set; }
        public int Records { get; set; }
        public int TotalRecords { get; set; }
    }
}
