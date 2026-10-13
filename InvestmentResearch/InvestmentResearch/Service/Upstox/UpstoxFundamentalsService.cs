using InvestmentResearch.Model.Upstox.Fundamental;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Facade service that coordinates multiple Upstox API endpoints for comprehensive fundamentals data
    /// </summary>
    public class UpstoxFundamentalsService : IUpstoxFundamentalsService
    {
        private readonly IUpstoxShareHoldingsService _shareHoldingsService;
        private readonly IUpstoxKeyRatiosService _keyRatiosService;
        private readonly IUpstoxIncomeStatementService _incomeStatementService;
        private readonly IUpstoxCorporateActionsService _corporateActionsService;
        private readonly IUpstoxCashFlowService _cashFlowService;
        private readonly IUpstoxBalanceSheetService _balanceSheetService;
        private readonly IUpstoxCompanyProfileService _companyProfileService;

        public UpstoxFundamentalsService(
            IUpstoxShareHoldingsService shareHoldingsService,
            IUpstoxKeyRatiosService keyRatiosService,
            IUpstoxIncomeStatementService incomeStatementService,
            IUpstoxCorporateActionsService corporateActionsService,
            IUpstoxCashFlowService cashFlowService,
            IUpstoxBalanceSheetService balanceSheetService,
            IUpstoxCompanyProfileService companyProfileService)
        {
            _shareHoldingsService = shareHoldingsService ?? throw new ArgumentNullException(nameof(shareHoldingsService));
            _keyRatiosService = keyRatiosService ?? throw new ArgumentNullException(nameof(keyRatiosService));
            _incomeStatementService = incomeStatementService ?? throw new ArgumentNullException(nameof(incomeStatementService));
            _corporateActionsService = corporateActionsService ?? throw new ArgumentNullException(nameof(corporateActionsService));
            _cashFlowService = cashFlowService ?? throw new ArgumentNullException(nameof(cashFlowService));
            _balanceSheetService = balanceSheetService ?? throw new ArgumentNullException(nameof(balanceSheetService));
            _companyProfileService = companyProfileService ?? throw new ArgumentNullException(nameof(companyProfileService));
        }

        public async Task<UpstoxFundamentalsData> GetAllFundamentalsAsync(string isin, string symbol, string companyName)
        {
            var fundamentalsData = new UpstoxFundamentalsData
            {
                Isin = isin,
                Symbol = symbol,
                CompanyName = companyName
            };

            try
            {
                Console.WriteLine($"Fetching fundamentals data for {symbol} (ISIN: {isin})...");
                isin = "INE918Z01012";
                // Fetch all data in parallel for better performance
                var shareHoldingsTask = _shareHoldingsService.GetShareHoldingsAsync(isin);
                var keyRatiosTask = _keyRatiosService.GetKeyRatiosAsync(isin);
                var incomeStatementTask = _incomeStatementService.GetIncomeStatementAsync(isin);
                var corporateActionsTask = _corporateActionsService.GetCorporateActionsAsync(isin);
                var cashFlowTask = _cashFlowService.GetCashFlowAsync(isin);
                var balanceSheetTask = _balanceSheetService.GetBalanceSheetAsync(isin);
                var companyProfileTask = _companyProfileService.GetCompanyProfileAsync(isin);

                await Task.WhenAll(
                    shareHoldingsTask, 
                    keyRatiosTask, 
                    incomeStatementTask,
                    corporateActionsTask,
                    cashFlowTask,
                    balanceSheetTask,
                    companyProfileTask
                    );

                var shareHoldings = await shareHoldingsTask;
                var keyRatios = await keyRatiosTask;
                var incomeStatement = await incomeStatementTask;
                var corporateActions = await corporateActionsTask;
                var cashFlow = await cashFlowTask;
                var balanceSheet = await balanceSheetTask;
                var companyProfile = await companyProfileTask;

                // Populate the master object
                fundamentalsData.ShareHoldings = shareHoldings;
                fundamentalsData.KeyRatios = keyRatios;
                fundamentalsData.IncomeStatement = incomeStatement;
                fundamentalsData.CorporateActions = corporateActions;
                fundamentalsData.CashFlow = cashFlow;
                fundamentalsData.BalanceSheet = balanceSheet;
                fundamentalsData.CompanyProfile = companyProfile;

                // Check if all data fetches were successful
                if (shareHoldings?.Status == "success" && 
                    keyRatios?.Status == "success" && 
                    incomeStatement?.Status == "success" &&
                    corporateActions?.Status == "success" &&
                    cashFlow?.Status == "success" &&
                    balanceSheet?.Status == "success" &&
                    companyProfile?.Status == "success")
                {
                    fundamentalsData.FetchStatus = "success";
                }
                else
                {
                    fundamentalsData.FetchStatus = "partial";
                    Console.WriteLine($"Warning: Some data could not be fetched for {symbol}");
                }

                Console.WriteLine($"Fundamentals data fetched successfully for {symbol}");
                var options = new JsonSerializerOptions { WriteIndented = true };
                string jsonString = JsonSerializer.Serialize(fundamentalsData, options);
                return fundamentalsData;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching all fundamentals for {symbol}: {ex.Message}");
                fundamentalsData.FetchStatus = "error";
                return fundamentalsData;
            }
        }
    }
}

