using InvestmentResearch.Model;
using InvestmentResearch.Repository;
using InvestmentResearch.Service.Upstox;

namespace InvestmentResearch.Service;

public class CompanyResearchService : ICompanyResearchService
{
    private ICompanyData _companyData;
    private readonly IFileHelper _fileHelper;
    private readonly IGeminiService _geminiService;
    private readonly IUpstoxCompanyProfileService _companyProfileService;
    private readonly IUpstoxShareHoldingsService _shareHoldingsService;
    private readonly IUpstoxIncomeStatementService _incomeStatementService;

    public CompanyResearchService(
        ICompanyData companyData,
        IFileHelper fileHelper,
        IGeminiService geminiService,
        IUpstoxCompanyProfileService companyProfileService,
        IUpstoxShareHoldingsService shareHoldingsService,
        IUpstoxIncomeStatementService incomeStatementService)
    {
        _companyData = companyData ?? throw new ArgumentNullException(nameof(companyData));
        _fileHelper = fileHelper;
        _geminiService = geminiService;
        _companyProfileService = companyProfileService ?? throw new ArgumentNullException(nameof(companyProfileService));
        _shareHoldingsService = shareHoldingsService ?? throw new ArgumentNullException(nameof(shareHoldingsService));
        _incomeStatementService = incomeStatementService ?? throw new ArgumentNullException(nameof(incomeStatementService));
    }

    public async Task<CompanyResearch?> RunReachModel(string Isin, string CompanyName, string Symbol)
    {
        var companyResearch = _companyData.GetCompanyDataByIsin(Isin);
        var isInserted = false;
        if (companyResearch == null)
        {
            isInserted = true;
            companyResearch = new()
            { 
                ISIN = Isin,
                Symbol = Symbol,
                CompanyName = CompanyName,
                CreateDate = DateTime.Now,
                NoOfAttempts = 1,
            };
        }
        else
        {
            companyResearch.NoOfAttempts = companyResearch.NoOfAttempts ?? 1 + 1;
            companyResearch.ModifyDate = DateTime.Now;
        }
        if (companyResearch.IsCompleted == true || companyResearch.IsWatchList == true)
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Company Research is completed or in watchlist.");
            return companyResearch;
        }
        
        if (companyResearch.IsCompanyProfileService == null || companyResearch.IsCompanyProfileService == false)
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Fetching Company Profile data from Upstox.");
            var companyProfile = await _companyProfileService.GetCompanyProfileAsync(Isin);
            if (companyProfile != null && companyProfile.Data != null && companyProfile.Data.SectorMarketCapInr.Value > 0)
            {
                companyResearch.Sector = companyProfile.Data.Sector;
                companyResearch.MarketCap = companyProfile.Data.SectorMarketCapInr.Value;
                companyResearch.IsCompanyProfileService = true;
                if (companyResearch.MarketCap > 10000)
                {
                    companyResearch.Reason = "Market Cap is greater than 10,000";
                    companyResearch.IsCompleted = true;
                    Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Market Cap is greater than 10,000");
                    return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
                }
                if(" IT - Software,Trading,Investment,Plastic Products,Bank".IndexOf(companyResearch.Sector) > 0)
                {
                    companyResearch.Reason = "Sector is " + companyResearch.Sector;
                    companyResearch.IsCompleted = true;
                    Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Sector is " + companyResearch.Sector);
                    return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
                }
            }
        }
        if (companyResearch.IsShareHoldingsService == null || companyResearch.IsShareHoldingsService == false)
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Fetching Share Holdings data from Upstox.");
            var shareHoldings = await _shareHoldingsService.GetShareHoldingsAsync(Isin);
            if (shareHoldings != null && shareHoldings.Data != null && shareHoldings.Data?.Promoters?.History != null && shareHoldings.Data?.Promoters?.History.Count != 0)
            {
                companyResearch.PromotersShare = GetLatestPromotersShare(shareHoldings.Data.Promoters.History);
                companyResearch.PromotersShare = companyResearch.PromotersShare == 0 ? null : companyResearch.PromotersShare;
                if (companyResearch.PromotersShare > 0)
                {
                    companyResearch.IsShareHoldingsService = true;
                    if (companyResearch.PromotersShare != null && companyResearch.PromotersShare < 50)
                    {
                        companyResearch.Reason = "Promoters Share is less than 50%";
                        companyResearch.IsCompleted = true;
                        Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Promoters Share is less than 50%");
                        return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
                    }
                }
            }
        }
        if (companyResearch.IsIncomeStatementService == null || companyResearch.IsIncomeStatementService == false)
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Fetching Income Statement data from Upstox.");
            var incomeStatement = await _incomeStatementService.GetIncomeStatementAsync(Isin);
            if (incomeStatement != null && incomeStatement.Data != null
                && (incomeStatement.Data?.IncomeStatement?.Revenue != null || incomeStatement.Data?.FullStatement?.ProfitAfterTax != null))
            {
                if (incomeStatement.Data?.FullStatement?.ProfitAfterTax?.History != null)
                {
                    var histories = incomeStatement.Data?.FullStatement?.ProfitAfterTax?.History;
                    decimal? profitAfterTax1, profitAfterTax2, profitAfterTax3, profitAfterTax4, profitAfterTax5;
                    (profitAfterTax1, profitAfterTax2, profitAfterTax3, profitAfterTax4, profitAfterTax5) = GetAllProfitAfterTax(histories);
                    companyResearch.ProfitAfterTax1 = profitAfterTax1;
                    companyResearch.ProfitAfterTax2 = profitAfterTax2;
                    companyResearch.ProfitAfterTax3 = profitAfterTax3;
                    companyResearch.ProfitAfterTax4 = profitAfterTax4;
                    companyResearch.ProfitAfterTax5 = profitAfterTax5;
                    var cagr = CalculateCagr(companyResearch.ProfitAfterTax1, companyResearch.ProfitAfterTax2, companyResearch.ProfitAfterTax3, companyResearch.ProfitAfterTax4, companyResearch.ProfitAfterTax5);
                    if (cagr != -1)
                    {
                        companyResearch.ProfitAfterTaxCagr = Convert.ToDecimal(cagr);
                    }
                }
                if (incomeStatement.Data?.IncomeStatement?.Revenue?.History != null)
                {
                    var histories = incomeStatement.Data?.IncomeStatement?.Revenue?.History;
                    decimal? totalRevenue1, totalRevenue2, totalRevenue3, totalRevenue4, totalRevenue5;
                    (totalRevenue1, totalRevenue2, totalRevenue3, totalRevenue4, totalRevenue5) = GetAllRevenue(histories);
                    companyResearch.TotalRevenue1 = totalRevenue1;
                    companyResearch.TotalRevenue2 = totalRevenue2;
                    companyResearch.TotalRevenue3 = totalRevenue3;
                    companyResearch.TotalRevenue4 = totalRevenue4;
                    companyResearch.TotalRevenue5 = totalRevenue5;
                    var cagr = CalculateCagr(companyResearch.TotalRevenue1, companyResearch.TotalRevenue2, companyResearch.TotalRevenue3, companyResearch.TotalRevenue4, companyResearch.TotalRevenue5);
                    if (cagr != -1)
                    {
                        companyResearch.TotalRevenueCagr = Convert.ToDecimal(cagr);
                    }
                }
                if (companyResearch.TotalRevenueCagr != null && companyResearch.ProfitAfterTaxCagr != null
                    && companyResearch.TotalRevenueCagr != -1 && companyResearch.ProfitAfterTaxCagr != -1)
                {
                    companyResearch.IsIncomeStatementService = true;
                    if (companyResearch.TotalRevenueCagr < 25 && companyResearch.ProfitAfterTaxCagr < 25)
                    {
                        companyResearch.Reason = "Revenue and Profit After Tax CAGR are less than 25%";
                        companyResearch.IsCompleted = true;
                        Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Revenue and Profit After Tax CAGR are less than 25%");
                        return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
                    }
                }
                    
            }
        }
        if (companyResearch.CompanyEstablished == null || companyResearch.CompanyEstablished == 0)
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Fetching Company Established data from Gemini.");
            var systemInstruction = await this._fileHelper.GetPrompt("System_Company_Established.txt", sectorName: null!, companyName: companyResearch.CompanyName.Replace(" IPO", "",StringComparison.InvariantCultureIgnoreCase).Replace("-ipo", "", StringComparison.InvariantCultureIgnoreCase), InputData: null!);
            var userData = await this._fileHelper.GetPrompt("User_Company_Established.txt", sectorName: null!, companyName: companyResearch.CompanyName.Replace(" IPO", "", StringComparison.InvariantCultureIgnoreCase).Replace("-ipo", "", StringComparison.InvariantCultureIgnoreCase), InputData: null!);
            var jsonPayload = await _geminiService.GenerateContentAsync(systemInstruction: systemInstruction, userData: userData);
            if (!string.IsNullOrEmpty(jsonPayload) && int.TryParse(jsonPayload, out int companyEstablished) && companyEstablished > 0)
            {
                companyResearch.CompanyEstablished = companyEstablished;
                if (DateTime.Now.Year - companyEstablished > 20)
                {
                    companyResearch.Reason = "Company is more than 20 years old";
                    companyResearch.IsCompleted = true;
                    Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Company is more than 20 years old");
                    return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
                }
            }
        }
        if (companyResearch.CompanyEstablished != null && companyResearch.CompanyEstablished > 0 
            && companyResearch.IsCompanyProfileService != null && companyResearch.IsCompanyProfileService == true
            && companyResearch.IsShareHoldingsService != null && companyResearch.IsShareHoldingsService == true
            && companyResearch.IsIncomeStatementService != null && companyResearch.IsIncomeStatementService == true
            && string.IsNullOrEmpty(companyResearch.Reason))
        {
            Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Company Research is completed. Need further investigation.");
            companyResearch.IsCompleted = true;
        }

        Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## {companyResearch.CompanyName} :-Company Research is completed. Some information is missing.");
        return isInserted ? _companyData.InsertCompanyDate(companyResearch) : _companyData.UpdateCompanyData(companyResearch);
    }

    private decimal GetLatestPromotersShare(List<Model.Upstox.Fundamental.ShareHolding.History> histories)
    {
        decimal latestPromotersShare = 0;
        DateTime latestDate = DateTime.MinValue;
        foreach (var history in histories)
        {
            if (DateTime.TryParse("01 " + history.Period, out DateTime date))
            {
                if (date > latestDate)
                {
                    latestDate = date;
                    latestPromotersShare = history.Value;
                }
            }
        }
        return latestPromotersShare;
    }

    private (decimal?, decimal?, decimal?, decimal?, decimal?) GetAllProfitAfterTax(List<Model.Upstox.Fundamental.IncomeStatement.FullStatementHistory> histories)
    {
        List<DateValue> dateValues = new List<DateValue>();
        foreach (var history in histories)
        {
            if (DateTime.TryParse("01 " + history.Period, out DateTime date))
            {
                dateValues.Add(new DateValue { date = date, value = history.Value });
            }
        }

        return SeprateValues(dateValues);
    }

    private (decimal?, decimal?, decimal?, decimal?, decimal?) GetAllRevenue(List<Model.Upstox.Fundamental.IncomeStatement.History> histories)
    {
        List<DateValue> dateValues = new List<DateValue>();
        foreach (var history in histories)
        {
            if (DateTime.TryParse("01 " + history.Period, out DateTime date))
            {
                dateValues.Add(new DateValue { date = date, value = history.Value });
            }
        }

        return SeprateValues(dateValues);
    }

    private (decimal?, decimal?, decimal?, decimal?, decimal?) SeprateValues(List<DateValue> dateValues)
    {
        decimal? value1 = null, value2 = null, value3 = null, value4 = null, value5 = null;
        if (dateValues.Count > 0)
        {
            dateValues.Sort((x, y) => y.date.CompareTo(x.date)); // Sort in descending order
            if (dateValues.Count > 0) value1 = dateValues[0].value;
            if (dateValues.Count > 1) value2 = dateValues[1].value;
            if (dateValues.Count > 2) value3 = dateValues[2].value;
            if (dateValues.Count > 3) value4 = dateValues[3].value;
            if (dateValues.Count > 4) value5 = dateValues[4].value;
        }

        return (value1, value2, value3, value4, value5);
    }

    private double CalculateCagr(decimal? value1, decimal? value2, decimal? value3, decimal? value4, decimal? value5)
    {
        double endingValue = 0, beginningValue = 0;
        int years = 0;
        if (value1.HasValue)
        {
            endingValue = Convert.ToDouble(value1.Value);
            beginningValue = Convert.ToDouble(value1.Value);
            years++;
        }
        if (value2.HasValue)
        {
            beginningValue = Convert.ToDouble(value2.Value);
            years++;
        }
        if (value3.HasValue)
        {
            beginningValue = Convert.ToDouble(value3.Value);
            years++;
        }
        if (value4.HasValue)
        {
            beginningValue = Convert.ToDouble(value4.Value);
            years++;
        }
        if (value5.HasValue)
        {
            beginningValue = Convert.ToDouble(value5.Value);
            years++;
        }
        if (years == 0 || years == 1 || years == 2)
        {
            return -1;
        }
        else if (endingValue == beginningValue)
        {
            return 0;
        }
        else if (endingValue <= 0)
        {
            return 0;
        }
        else if(beginningValue < 0)
        {
            var difference = endingValue - beginningValue;
            var absoluteBase = Math.Abs(beginningValue);
            var totalPercentage = (difference * 100) / absoluteBase;
            return totalPercentage / years;
        }
        else
        {
            // Core CAGR Formula: (EndingValue / BeginningValue) ^ (1 / Years) - 1
            var carg = Math.Pow(endingValue / beginningValue, 1.0 / years) - 1;
            return carg * 100;
        }
    }
}

public class DateValue
{
    public DateTime date { get; set; }
    public decimal? value { get; set; }
}