namespace InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;

public class History
{
    public decimal TotalAsset { get; set; }
    public decimal TotalLiability { get; set; }
    public string Period { get; set; } = string.Empty;
}
