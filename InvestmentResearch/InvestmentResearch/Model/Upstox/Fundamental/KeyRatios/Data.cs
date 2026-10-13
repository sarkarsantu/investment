namespace InvestmentResearch.Model.Upstox.Fundamental.KeyRatios;

public class Data
{
    public PE PE { get; set; } = new();
    public PB PB { get; set; } = new();
    public ROA ROA { get; set; } = new();
    public ROE ROE { get; set; } = new();
    public ROCE ROCE { get; set; } = new();
    public EBITDA EBITDA { get; set; } = new();
    public QuickRatio QuickRatio { get; set; } = new();
}
