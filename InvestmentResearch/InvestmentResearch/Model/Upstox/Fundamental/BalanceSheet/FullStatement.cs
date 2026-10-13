namespace InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;

public class FullStatement
{
    public NonCurrentAssets NonCurrentAssets { get; set; } = new();
    public CurrentAssets CurrentAssets { get; set; } = new();
    public TotalAssets TotalAssets { get; set; } = new();
    public CurrentLiability CurrentLiability { get; set; } = new();
    public NetCurrentAsset NetCurrentAsset { get; set; } = new();
    public NonCurrentLiabilities NonCurrentLiabilities { get; set; } = new();
    public EquityCapital EquityCapital { get; set; } = new();
    public TotalEquityAndLiability TotalEquityAndLiability { get; set; } = new();
}
