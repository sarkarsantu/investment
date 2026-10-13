namespace InvestmentResearch.Model.Upstox.Fundamental.CashFlow;

public class FullStatement
{
    public ProfitBeforeTax ProfitBeforeTax { get; set; } = new();
    public IncomeBeforeWCChange IncomeBeforeWCChange { get; set; } = new();
    public ChangeInAsset ChangeInAsset { get; set; } = new();
    public ChangeInLiability ChangeInLiability { get; set; } = new();
    public ChangeInWC ChangeInWC { get; set; } = new();
    public CashFlowFromOperation CashFlowFromOperation { get; set; } = new();
    public CashFlowFromInvesting CashFlowFromInvesting { get; set; } = new();
    public CashFlowFromFinancing CashFlowFromFinancing { get; set; } = new();
    public TotalCashFlow TotalCashFlow { get; set; } = new();
    public CashStartOfTheYear CashStartOfTheYear { get; set; } = new();
    public CashEndOfTheYear CashEndOfTheYear { get; set; } = new();
}
