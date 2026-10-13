using System.Text.Json.Serialization;

namespace InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;

public class MetaData
{
    [JsonPropertyName("page")]
    public PageInfo Page { get; set; }
}
