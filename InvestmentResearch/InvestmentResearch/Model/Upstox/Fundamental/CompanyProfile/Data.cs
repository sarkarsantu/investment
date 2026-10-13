namespace InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;

public class Data
{
    public string CompanyProfile { get; set; } = string.Empty;
    public string Sector { get; set; } = string.Empty;
    public MarketCapInfo SectorMarketCapInr { get; set; } = new MarketCapInfo();
    public MarketCapInfo SectorMarketCapUsd { get; set; } = new MarketCapInfo();
}
