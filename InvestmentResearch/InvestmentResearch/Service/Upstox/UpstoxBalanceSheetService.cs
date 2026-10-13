using InvestmentResearch.Model.Upstox.Fundamental.BalanceSheet;
using InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching balance sheet data from Upstox API
    /// </summary>
    public class UpstoxBalanceSheetService : UpstoxApiServiceBase, IUpstoxBalanceSheetService
    {
        public UpstoxBalanceSheetService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves consolidated balance sheet for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Balance sheet response containing assets, liabilities, and equity details</returns>
        public async Task<BalanceSheetResponse> GetBalanceSheetAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/balance-sheet?type=consolidated&fs=true";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new BalanceSheetResponse { Status = "error" };

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new BalanceSheetResponse { Status = "error" };

                        var response = new BalanceSheetResponse { Status = "success" };
                        var data = new Model.Upstox.Fundamental.BalanceSheet.Data();

                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            if (dataElement.TryGetProperty("type", out var typeElem))
                                data.Type = typeElem.GetString();

                            if (dataElement.TryGetProperty("time_period", out var timePeriodElem))
                                data.TimePeriod = timePeriodElem.GetString();

                            if (dataElement.TryGetProperty("units_in", out var unitsElem))
                                data.UnitsIn = unitsElem.GetString();

                            // Parse history array
                            if (dataElement.TryGetProperty("history", out var historyElem) &&
                                historyElem.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var historyItem in historyElem.EnumerateArray())
                                {
                                    var assetLiabilityHistory = new AssetLiability
                                    {
                                        TotalAsset = historyItem.TryGetProperty("total_asset", out var assetElem) ? TryParseDecimalValue(assetElem) : 0m,
                                        TotalLiabilities = historyItem.TryGetProperty("total_liability", out var liabilityElem) ? TryParseDecimalValue(liabilityElem) : 0m,
                                        Period = historyItem.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty
                                    };
                                    data.AssetLiability.Add(assetLiabilityHistory);
                                }
                            }

                            // Parse full_statement array
                            if (dataElement.TryGetProperty("full_statement", out var fullStatementElem) &&
                                fullStatementElem.ValueKind == JsonValueKind.Array)
                            {
                                foreach (var particularItem in fullStatementElem.EnumerateArray())
                                {
                                    string particularName = particularItem.TryGetProperty("particular", out var catElem) ? catElem.GetString() : string.Empty;
                                    var history = new List<FullStatementHistory>();

                                    if (particularItem.TryGetProperty("history", out var historyElement) &&
                                    historyElement.ValueKind == JsonValueKind.Array)
                                    {
                                        foreach (var entry in historyElement.EnumerateArray())
                                        {
                                            var historyEntry = new FullStatementHistory
                                            {
                                                Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty,
                                                Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m
                                            };
                                            history.Add(historyEntry);
                                        }
                                    }

                                    // Map each category to the corresponding property
                                    switch (particularName?.ToLower())
                                    {
                                        case "non-current assets":
                                            data.FullStatement.NonCurrentAssets = new NonCurrentAssets { History = history };
                                            break;
                                        case "current assets":
                                            data.FullStatement.CurrentAssets = new CurrentAssets { History = history };
                                            break;
                                        case "total assets":
                                            data.FullStatement.TotalAssets = new TotalAssets { History = history };
                                            break;
                                        case "current liabilities":
                                            data.FullStatement.CurrentLiability = new CurrentLiability { History = history };
                                            break;
                                        case "net current asset":
                                            data.FullStatement.NetCurrentAsset = new NetCurrentAsset { History = history };
                                            break;
                                        case "non-current liabilities":
                                            data.FullStatement.NonCurrentLiabilities = new NonCurrentLiabilities { History = history };
                                            break;
                                        case "equity capital":
                                            data.FullStatement.EquityCapital = new EquityCapital { History = history };
                                            break;
                                        case "total equity & liabilities":
                                            data.FullStatement.TotalEquityAndLiability = new TotalEquityAndLiability { History = history };
                                            break;
                                        default:
                                            {
                                                Console.WriteLine($"Unknown category '{particularName}' found in full_statement for ISIN {isin}");
                                                throw new Exception($"Unknown category '{particularName}' found in full_statement for ISIN {isin}");
                                            }
                                    }
                                }
                                
                            }
                        }

                        response.Data = data;
                        return response;
                    }
                }

                return new BalanceSheetResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching balance sheet for ISIN {isin}: {ex.Message}");
                return new BalanceSheetResponse { Status = "error" };
            }
        }
    }
}
