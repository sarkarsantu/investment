using InvestmentResearch.Model.Upstox;
using InvestmentResearch.Model.Upstox.Fundamental.ShareHolding;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching share holding data from Upstox API
    /// </summary>
    public class UpstoxShareHoldingsService : UpstoxApiServiceBase, IUpstoxShareHoldingsService
    {
        public UpstoxShareHoldingsService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves share holding information for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Share holding response containing detailed shareholding data by category</returns>
        public async Task<ShareHoldingResponse> GetShareHoldingsAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/share-holdings";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new ShareHoldingResponse { Status = "error" };

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new ShareHoldingResponse { Status = "error" };

                        var response = new ShareHoldingResponse { Status = "success" };
                        var data = new Model.Upstox.Fundamental.ShareHolding.Data();

                        if (root.TryGetProperty("data", out var dataElement) &&
                            dataElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var categoryItem in dataElement.EnumerateArray())
                            {
                                string category = categoryItem.TryGetProperty("category", out var catElem) ? catElem.GetString() : string.Empty;
                                var history = new List<History>();

                                if (categoryItem.TryGetProperty("history", out var historyElem) &&
                                    historyElem.ValueKind == JsonValueKind.Array)
                                {
                                    foreach (var entry in historyElem.EnumerateArray())
                                    {
                                        var historyEntry = new History
                                        {
                                            Period = entry.TryGetProperty("period", out var periodElem) ? periodElem.GetString() : string.Empty,
                                            Value = entry.TryGetProperty("value", out var valElem) ? TryParseDecimalValue(valElem) : 0m
                                        };
                                        history.Add(historyEntry);
                                    }
                                }

                                // Map each category to the corresponding property
                                switch (category?.ToLower())
                                {
                                    case "promoters":
                                        data.Promoters = new Promoter { History = history };
                                        break;
                                    case "fii":
                                        data.FII = new FII { History = history };
                                        break;
                                    case "other_dii":
                                        data.OtherDII = new OtherDII { History = history };
                                        break;
                                    case "mutual_funds":
                                        data.MutualFund = new MutualFund { History = history };
                                        break;
                                    case "retail_and_other":
                                        data.RetailAndOther = new RetailAndOther { History = history };
                                        break;
                                    default:
                                        {
                                            Console.WriteLine($"Warning: Unrecognized category '{category}' for ISIN {isin}");
                                            throw new InvalidOperationException($"Unrecognized category '{category}' in share holdings data for ISIN {isin}");
                                        }
                                      
                                }
                            }
                        }

                        response.Data = (data);
                        return response;
                    }
                }

                return new ShareHoldingResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching share holdings for ISIN {isin}: {ex.Message}");
                return new ShareHoldingResponse { Status = "error" };
            }
        }
    }
}
