namespace InvestmentResearch.Model;

public class CompanyResearch
{
    public long CompanyResearchID { get; set; }
    public string? ISIN { get; set; }
    public string? Symbol { get; set; }
    public string? CompanyName { get; set; }
    public bool? IsCompleted { get; set; }
    public string? Reason { get; set; }
    public bool? IsWatchList { get; set; }
    public int? CompanyEstablished { get; set; }
    public bool? IsCompanyProfileService { get; set; }
    public string? Sector { get; set; }
    public decimal? MarketCap { get; set; }
    public bool? IsShareHoldingsService { get; set; }
    public decimal? PromotersShare { get; set; }
    public bool? IsIncomeStatementService { get; set; }
    public decimal? ProfitAfterTax1 { get; set; }
    public decimal? ProfitAfterTax2 { get; set; }
    public decimal? ProfitAfterTax3 { get; set; }
    public decimal? ProfitAfterTax4 { get; set; }
    public decimal? ProfitAfterTax5 { get; set; }
    public decimal? ProfitAfterTaxCagr { get; set; }
    public decimal? TotalRevenue1 { get; set; }
    public decimal? TotalRevenue2 { get; set; }
    public decimal? TotalRevenue3 { get; set; }
    public decimal? TotalRevenue4 { get; set; }
    public decimal? TotalRevenue5 { get; set; }
    public decimal? TotalRevenueCagr { get; set; }
    public DateTime? CreateDate { get; set; }
    public DateTime? ModifyDate { get; set; }
    public int? NoOfAttempts { get; set; }
    public string? NameFromOutsource { get; set; }
}
