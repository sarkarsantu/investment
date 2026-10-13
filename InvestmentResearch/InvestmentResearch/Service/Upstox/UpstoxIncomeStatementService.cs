using InvestmentResearch.Model;
using InvestmentResearch.Model.Upstox.Fundamental.IncomeStatement;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching income statement data from Upstox API
    /// </summary>
    public class UpstoxIncomeStatementService : UpstoxApiServiceBase, IUpstoxIncomeStatementService
    {
        public UpstoxIncomeStatementService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves consolidated yearly income statement for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Income statement response containing revenue, profit, and other financial metrics</returns>
        public async Task<IncomeStatementResponse> GetIncomeStatementAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/income-statement?type=consolidated&time_period=yearly&fs=true";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new IncomeStatementResponse { Status = "error" };

                // Manual JSON parsing for income statement
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new IncomeStatementResponse { Status = "error" };

                        var response = new IncomeStatementResponse { Status = "success" };
                        var data = new Model.Upstox.Fundamental.IncomeStatement.Data();

                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            // Extract basic properties
                            if (dataElement.TryGetProperty("type", out var typeElem))
                                data.Type = typeElem.GetString();

                            if (dataElement.TryGetProperty("time_period", out var timePeriodElem))
                                data.TimePeriod = timePeriodElem.GetString();

                            if (dataElement.TryGetProperty("units_in", out var unitsElem))
                                data.UnitsIn = unitsElem.GetString();

                            // Parse income_statement array
                            if (dataElement.TryGetProperty("income_statement", out var incomeStatementElem) &&
                                incomeStatementElem.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var category in incomeStatementElem.EnumerateArray())
                                {
                                    string categoryName = category.TryGetProperty("category", out var catElem) ? catElem.GetString() : string.Empty;
                                    var history = new List<History>();
                                   
                                    if (category.TryGetProperty("history", out var historyElem) &&
                                        historyElem.ValueKind == JsonValueKind.Array)
                                    {
                                        foreach (var entry in historyElem.EnumerateArray())
                                        {
                                            var historyEntry = new History
                                            {
                                                Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m,
                                                Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty,
                                                Change = entry.TryGetProperty("change", out var changeElem) ? changeElem.GetString() : string.Empty
                                            };
                                            history.Add(historyEntry);
                                        }
                                    }
                                    // Map each category to the corresponding property
                                    switch (categoryName?.ToLower())
                                    {
                                        case "revenue":
                                            data.IncomeStatement.Revenue = new IncomeStatementRevenue { History = history };
                                            break;
                                        case "operating_profit":
                                            data.IncomeStatement.OperatingProfit = new IncomeStatementOperatingProfit { History = history };
                                            break;
                                        case "net_profit":
                                            data.IncomeStatement.NetProfit = new IncomeStatementNetProfit { History = history };
                                            break;
                                        default:
                                            {
                                               Console.WriteLine($"Unknown category '{categoryName}' encountered in income_statement.");
                                                throw new InvalidOperationException($"Unknown category '{categoryName}' encountered in income_statement.");
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
                                    var history = new List<FullStatementHistory>();

                                    if (particular.TryGetProperty("history", out var historyElem) &&
                                        historyElem.ValueKind == JsonValueKind.Array)
                                    {
                                        foreach (var entry in historyElem.EnumerateArray())
                                        {
                                            var historyEntry = new FullStatementHistory
                                            {
                                                Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m,
                                                Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty
                                            };
                                            history.Add(historyEntry);
                                        }
                                    }

                                    // Map each category to the corresponding property
                                    switch (particularName?.ToLower())
                                    {
                                        case "revenue":
                                            data.FullStatement.Revenue = new Revenue { History = history };
                                            break;
                                        case "other income":
                                            data.FullStatement.OtherIncome = new OtherIncome { History = history };
                                            break;
                                        case "total revenue":
                                            data.FullStatement.TotalRevenue = new TotalRevenue { History = history };
                                            break;
                                        case "total expenses":
                                            data.FullStatement.TotalExpenses = new TotalExpenses { History = history };
                                            break;
                                        case "profit before tax":
                                            data.FullStatement.ProfitBeforeTax = new ProfitBeforeTax { History = history };
                                            break;
                                        case "tax":
                                            data.FullStatement.Tax = new Tax { History = history };
                                            break;
                                        case "profit after tax":
                                            data.FullStatement.ProfitAfterTax = new ProfitAfterTax { History = history };
                                            break;
                                        case "eps - basic":
                                            data.FullStatement.EPSBasic = new EPSBasic { History = history };
                                            break;
                                        case "eps - diluted":
                                            data.FullStatement.EPSDiluted = new EPSDiluted { History = history };
                                            break;
                                        default:
                                            {
                                                Console.WriteLine($"Unknown particular '{particularName}' encountered in full_statement.");
                                                throw new InvalidOperationException($"Unknown particular '{particularName}' encountered in full_statement.");
                                            }

                                    }
                                }
                            }
                        }

                        response.Data = data;
                        return response;
                    }
                }

                return new IncomeStatementResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching income statement for ISIN {isin}: {ex.Message}");
                return new IncomeStatementResponse { Status = "error" };
            }
        }
    }
}
