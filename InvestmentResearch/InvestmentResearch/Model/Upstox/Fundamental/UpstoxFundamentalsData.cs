using InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;
using InvestmentResearch.Model.Upstox.Fundamental.CashFlow;
using InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;
using InvestmentResearch.Model.Upstox.Fundamental.CorporateActions;
using InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;
using InvestmentResearch.Model.Upstox.Fundamental.KeyRatios;
using InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;

namespace InvestmentResearch.Model.Upstox.Fundamental;

/// <summary>
/// Master class to hold all Upstox fundamental data for a company
/// </summary>
public class UpstoxFundamentalsData
{
    public string Isin { get; set; } = string.Empty;
    public string Symbol { get; set; } = string.Empty;
    public string CompanyName { get; set; } = string.Empty;

    // Share Holdings Data
    public ShareHoldingResponse ShareHoldings { get; set; } = new ShareHoldingResponse();

    // Key Ratios Data
    public KeyRatiosResponse KeyRatios { get; set; } = new KeyRatiosResponse();

    // Income Statement Data
    public IncomeStatementResponse IncomeStatement { get; set; } = new IncomeStatementResponse();

    // Corporate Actions Data
    public CorporateActionsResponse CorporateActions { get; set; } = new CorporateActionsResponse();

    // Cash Flow Data
    public CashFlowResponse CashFlow { get; set; } = new CashFlowResponse();

    // Balance Sheet Data
    public BalanceSheetResponse BalanceSheet { get; set; } = new BalanceSheetResponse();

    // Company Profile Data
    public CompanyProfileResponse CompanyProfile { get; set; } = new CompanyProfileResponse();

    // Metadata
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;
    public string FetchStatus { get; set; } = string.Empty;
}
