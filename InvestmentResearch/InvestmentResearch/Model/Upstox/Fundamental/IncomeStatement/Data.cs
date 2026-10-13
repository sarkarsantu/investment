namespace InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;

public class Data
{
    public string Type { get; set; } = string.Empty;
    public string TimePeriod { get; set; } = string.Empty;
    public string UnitsIn { get; set; } = string.Empty;
    public IncomeStatement IncomeStatement { get; set; } = new IncomeStatement();
    public FullStatement FullStatement { get; set; } = new FullStatement();
}
