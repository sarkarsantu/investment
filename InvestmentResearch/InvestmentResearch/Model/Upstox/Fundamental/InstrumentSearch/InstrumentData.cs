using System.Text.Json.Serialization;

namespace InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;

public class InstrumentData
{
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("segment")]
    public string Segment { get; set; }

    [JsonPropertyName("exchange")]
    public string Exchange { get; set; }

    [JsonPropertyName("isin")]
    public string Isin { get; set; }

    [JsonPropertyName("instrument_key")]
    public string InstrumentKey { get; set; }

    [JsonPropertyName("exchange_token")]
    public string ExchangeToken { get; set; }

    [JsonPropertyName("trading_symbol")]
    public string TradingSymbol { get; set; }

    [JsonPropertyName("short_name")]
    public string ShortName { get; set; }

    [JsonPropertyName("tick_size")]
    public decimal TickSize { get; set; }

    [JsonPropertyName("lot_size")]
    public int LotSize { get; set; }

    [JsonPropertyName("instrument_type")]
    public string InstrumentType { get; set; }

    [JsonPropertyName("freeze_quantity")]
    public decimal FreezeQuantity { get; set; }

    [JsonPropertyName("qty_multiplier")]
    public int QtyMultiplier { get; set; }

    [JsonPropertyName("security_type")]
    public string SecurityType { get; set; }

    [JsonPropertyName("cas_eligible")]
    public bool CasEligible { get; set; }

    [JsonPropertyName("expiry")]
    public DateTime? Expiry { get; set; }

    [JsonPropertyName("weekly")]
    public bool? Weekly { get; set; }

    [JsonPropertyName("underlying_key")]
    public string UnderlyingKey { get; set; }

    [JsonPropertyName("underlying_type")]
    public string UnderlyingType { get; set; }

    [JsonPropertyName("underlying_symbol")]
    public string UnderlyingSymbol { get; set; }

    [JsonPropertyName("strike_price")]
    public decimal? StrikePrice { get; set; }

    [JsonPropertyName("minimum_lot")]
    public int? MinimumLot { get; set; }
}
