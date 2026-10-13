namespace InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;

public class IncomeStatementResponse
{
    public string Status { get; set; } = string.Empty;
    public Data Data { get; set; } = new Data();
}
