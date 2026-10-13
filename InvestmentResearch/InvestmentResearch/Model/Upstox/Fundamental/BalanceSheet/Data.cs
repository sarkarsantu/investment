namespace InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;

public class Data
{
    public string Type { get; set; } = string.Empty;
    public string TimePeriod { get; set; } = string.Empty;
    public string UnitsIn { get; set; } = string.Empty;
    public List<AssetLiability> AssetLiability { get; set; } = new List<AssetLiability>();
    public FullStatement FullStatement { get; set; } = new FullStatement();
}
