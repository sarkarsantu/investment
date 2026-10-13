using System.Text.Json.Serialization;

namespace InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;

public class PageInfo
{
    [JsonPropertyName("page_number")]
    public int PageNumber { get; set; }

    [JsonPropertyName("total_pages")]
    public int TotalPages { get; set; }

    [JsonPropertyName("records")]
    public int Records { get; set; }

    [JsonPropertyName("total_records")]
    public int TotalRecords { get; set; }
}
