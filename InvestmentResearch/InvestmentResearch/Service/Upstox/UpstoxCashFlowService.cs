using InvestmentResearch.Model.Upstox.Fundamental.CashFlow;
using InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching cash flow data from Upstox API
    /// </summary>
    public class UpstoxCashFlowService : UpstoxApiServiceBase, IUpstoxCashFlowService
    {
        public UpstoxCashFlowService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves consolidated yearly cash flow statement for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Cash flow response containing operating, investing, and financing cash flows</returns>
        public async Task<CashFlowResponse> GetCashFlowAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/cash-flow?type=consolidated&fs=true";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new CashFlowResponse { Status = "error" };

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new CashFlowResponse { Status = "error" };

                        var response = new CashFlowResponse { Status = "success" };
                        var data = new Model.Upstox.Fundamental.CashFlow.Data();

                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            if (dataElement.TryGetProperty("type", out var typeElem))
                                data.Type = typeElem.GetString();

                            if (dataElement.TryGetProperty("time_period", out var timePeriodElem))
                                data.TimePeriod = timePeriodElem.GetString();

                            if (dataElement.TryGetProperty("units_in", out var unitsElem))
                                data.UnitsIn = unitsElem.GetString();

                            // Parse cash_flow array
                            if (dataElement.TryGetProperty("cash_flow", out var cashFlowElem) &&
                                cashFlowElem.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var category in cashFlowElem.EnumerateArray())
                                {
                                    string categoryName = category.TryGetProperty("category", out var catElem) ? catElem.GetString() : string.Empty;
                                    var history = new List<CashFlowHistory>();
                                    
                                    if (category.TryGetProperty("history", out var historyElem) &&
                                        historyElem.ValueKind == JsonValueKind.Array)
                                    {
                                        foreach (var entry in historyElem.EnumerateArray())
                                        {
                                            var historyEntry = new CashFlowHistory
                                            {
                                                Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m,
                                                Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty,
                                                Change = entry.TryGetProperty("change", out var changeElem) ? changeElem.GetString() : string.Empty
                                            };
                                            history.Add(historyEntry);
                                        }

                                        // Map each category to the corresponding property
                                        switch (categoryName?.ToLower())
                                        {
                                            case "operating":
                                                data.CashFlow.Operating = new Operating { History = history };
                                                break;
                                            case "investing":
                                                data.CashFlow.Investing = new Investing { History = history };
                                                break;
                                            case "financing":
                                                data.CashFlow.Financing = new Financing { History = history };
                                                break;
                                            default:
                                                {
                                                    Console.WriteLine($"Unknown cash flow category: {categoryName} for ISIN {isin}");
                                                    throw new Exception($"Unknown cash flow category: {categoryName} for ISIN {isin}");
                                                }
                                        }
                                    }
                                }
                            }

                            // Parse full_statement array
                            if (dataElement.TryGetProperty("full_statement", out var fullStatementElem) &&
                                fullStatementElem.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var particular in fullStatementElem.EnumerateArray())
                                {
                                    string particularName = particular.TryGetProperty("particular", out var particularElem) ? particularElem.GetString() : string.Empty;
                                    var history = new List<Model.Upstox.Fundamental.CashFlow.History>();
                                    
                                    if (particular.TryGetProperty("history", out var historyElem) &&
                                        historyElem.ValueKind == JsonValueKind.Array)
                                    {
                                        foreach (var entry in historyElem.EnumerateArray())
                                        {
                                            var historyEntry = new Model.Upstox.Fundamental.CashFlow.History
                                            {
                                                Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m,
                                                Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty
                                            };
                                            history.Add(historyEntry);
                                        }
                                        // Map each category to the corresponding property
                                        switch (particularName?.ToLower())
                                        {
                                            case "profit before tax":
                                                data.FullStatement.ProfitBeforeTax = new ProfitBeforeTax { History = history };
                                                break;
                                            case "income before wc changes":
                                                data.FullStatement.IncomeBeforeWCChange = new IncomeBeforeWCChange { History = history };
                                                break;
                                            case "change in assets":
                                                data.FullStatement.ChangeInAsset = new ChangeInAsset { History = history };
                                                break;
                                            case "change in liabilities":
                                                data.FullStatement.ChangeInLiability = new ChangeInLiability { History = history };
                                                break;
                                            case "change in wc":
                                                data.FullStatement.ChangeInWC = new ChangeInWC { History = history };
                                                break;
                                            case "cash flow from operations":
                                                data.FullStatement.CashFlowFromOperation = new CashFlowFromOperation { History = history };
                                                break;
                                            case "cash flow from investing":
                                                data.FullStatement.CashFlowFromInvesting = new CashFlowFromInvesting { History = history };
                                                break;
                                            case "cash flow from financing":
                                                data.FullStatement.CashFlowFromFinancing = new CashFlowFromFinancing { History = history };
                                                break;
                                            case "total cash flow":
                                                data.FullStatement.TotalCashFlow = new TotalCashFlow { History = history };
                                                break;
                                            case "cash (start of the year)":
                                                data.FullStatement.CashStartOfTheYear = new CashStartOfTheYear { History = history };
                                                break;
                                            case "cash (end of the year)":
                                                data.FullStatement.CashEndOfTheYear = new CashEndOfTheYear { History = history };
                                                break;
                                            default:
                                                {
                                                    Console.WriteLine($"Unknown full statement particular: {particularName} for ISIN {isin}");
                                                    throw new Exception($"Unknown full statement particular: {particularName} for ISIN {isin}");
                                                }

                                        }
                                    }
                                }
                            }
                        }

                        response.Data = data;
                        return response;
                    }
                }

                return new CashFlowResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cash flow for ISIN {isin}: {ex.Message}");
                return new CashFlowResponse { Status = "error" };
            }
        }
    }
}
