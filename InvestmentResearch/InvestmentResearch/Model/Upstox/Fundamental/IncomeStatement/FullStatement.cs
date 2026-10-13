namespace InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;

public class FullStatement
{
    public Revenue Revenue { get; set; } = new();
    public OtherIncome OtherIncome { get; set; } = new();
    public TotalRevenue TotalRevenue { get; set; } = new();
    public TotalExpenses TotalExpenses { get; set; } = new();
    public ProfitBeforeTax ProfitBeforeTax { get; set; } = new();
    public Tax Tax { get; set; } = new();
    public ProfitAfterTax ProfitAfterTax { get; set; } = new();
    public EPSBasic EPSBasic { get; set; } = new();
    public EPSDiluted EPSDiluted { get; set; } = new();
}
