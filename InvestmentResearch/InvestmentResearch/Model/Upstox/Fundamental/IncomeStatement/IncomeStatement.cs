namespace InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;

public class IncomeStatement
{
    public IncomeStatementRevenue Revenue { get; set; } = new();
    public IncomeStatementOperatingProfit OperatingProfit { get; set; } = new();
    public IncomeStatementNetProfit NetProfit { get; set; } = new();
}
