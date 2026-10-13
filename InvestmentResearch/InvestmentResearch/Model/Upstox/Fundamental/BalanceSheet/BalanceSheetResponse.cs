namespace InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;

public class BalanceSheetResponse
{
    public string Status { get; set; } = string.Empty;
    public Data Data { get; set; } = new Data();
}
