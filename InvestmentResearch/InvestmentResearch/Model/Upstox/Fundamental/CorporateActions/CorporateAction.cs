namespace InvestmentResearch.Model.Upstox.Fundamental.CorporateActions;

public class CorporateAction
{
    public string Name { get; set; } = string.Empty;
    public string ExpiryDate { get; set; } = string.Empty;
    public decimal? Amount { get; set; }
    public decimal? Ratio { get; set; }
    public List<EventDetail> EventDetails { get; set; } = new List<EventDetail>();
}
