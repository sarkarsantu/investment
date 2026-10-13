namespace InvestmentResearch.Model.Upstox.Fundamental.CorporateActions;

public class CorporateActionsResponse
{
    public string Status { get; set; } = string.Empty;
    public List<CorporateAction> Data { get; set; } = new List<CorporateAction>();
}
