namespace InvestmentResearch.Model.Upstox.Fundamental.CashFlow;

public class CashFlow
{
    public Operating Operating { get; set; } = new();
    public Investing Investing { get; set; } = new();
    public Financing Financing { get; set; } = new();
}
