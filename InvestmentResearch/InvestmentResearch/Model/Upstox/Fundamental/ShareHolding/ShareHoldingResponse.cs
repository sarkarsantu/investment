namespace InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;

public class ShareHoldingResponse
{
    public string Status { get; set; } = string.Empty;
    public Data Data { get; set; } = new Data();
}