namespace InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;

public class Data
{
    public Promoter Promoters { get; set; } = new();
    public FII FII { get; set; } = new();
    public OtherDII OtherDII { get; set; } = new();
    public MutualFund MutualFund { get; set; } = new();
    public RetailAndOther RetailAndOther { get; set; } = new();
}