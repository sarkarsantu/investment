using InvestmentResearch.Model.Upstox.Fundamental.CorporateActions;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching corporate actions data from Upstox API
    /// </summary>
    public class UpstoxCorporateActionsService : UpstoxApiServiceBase, IUpstoxCorporateActionsService
    {
        public UpstoxCorporateActionsService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves corporate actions (dividends, splits, etc.) for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Corporate actions response containing dividend and other corporate action details</returns>
        public async Task<CorporateActionsResponse> GetCorporateActionsAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/corporate-actions";
                var response = await GetApiResponseAsync(endpoint);

                if (response == null)
                    return new CorporateActionsResponse { Status = "error" };

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                return JsonSerializer.Deserialize<CorporateActionsResponse>(response, options) ?? new CorporateActionsResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching corporate actions for ISIN {isin}: {ex.Message}");
                return new CorporateActionsResponse { Status = "error" };
            }
        }
    }
}
