using System.Text.Json;
using InvestmentResearch.Model.Upstox.IPO;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching IPO data from Upstox API
    /// </summary>
    public class UpstoxIPOService : UpstoxApiServiceBase, IUpstoxIPOService
    {
        public UpstoxIPOService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves list of open IPOs available for subscription
        /// </summary>
        /// <returns>List of open IPO data</returns>
        public async Task<List<IPOData>> GetOpenIPOsAsync()
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/ipos?status=open&issue_type=regular&page_number=1&records=30";

                var content = await GetApiResponseAsync(endpoint);
                if (content == null)
                    return new List<IPOData>();

                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var ipoResponse = JsonSerializer.Deserialize<UpstoxIPOResponse>(content, options);

                return ipoResponse?.Data ?? new List<IPOData>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching IPO data from Upstox API: {ex.Message}");
                return new List<IPOData>();
            }
        }
    }
}
