namespace InvestmentResearch.Model.Upstox.Fundamental.CashFlow;

public class CashFlowHistory
{
    public decimal Value { get; set; }
    public string Period { get; set; } = string.Empty;
    public string Change { get; set; } = string.Empty;
}
