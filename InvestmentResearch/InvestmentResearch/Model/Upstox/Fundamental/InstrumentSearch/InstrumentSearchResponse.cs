using System.Text.Json.Serialization;

namespace InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;

public class InstrumentSearchResponse
{
    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("data")]
    public List<InstrumentData> Data { get; set; }

    [JsonPropertyName("meta_data")]
    public MetaData MetaData { get; set; }
}
