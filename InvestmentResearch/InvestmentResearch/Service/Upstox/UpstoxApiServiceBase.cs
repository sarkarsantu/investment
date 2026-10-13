using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Base class for Upstox API services providing common functionality
    /// </summary>
    public abstract class UpstoxApiServiceBase
    {
        protected readonly HttpClient HttpClient;
        protected const string ApiBaseUrl = "https://api.upstox.com/v2";
        protected const string BearerToken = "eyJ0eXAiOiJKV1QiLCJrZXlfaWQiOiJza192MS4wIiwiYWxnIjoiSFMyNTYifQ.eyJzdWIiOiI4NkJCNjQiLCJqdGkiOiI2YWJjYTQ0ZTM0Y2FlNDdlM2UwNzExZjAiLCJpc011bHRpQ2xpZW50IjpmYWxzZSwiaXNQbHVzUGxhbiI6ZmFsc2UsImlhdCI6MTc5MDc0NzcyNiwiaXNzIjoidWRhcGktZ2F0ZXdheS1zZXJ2aWNlIiwiZXhwIjoxNzkwODA1NjAwfQ.oFdzf6TYJZiW17Yk38X0h0Ha5NJHD3FZmn6ytNOdqN4";

        protected UpstoxApiServiceBase(HttpClient httpClient)
        {
            HttpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Sends an HTTP GET request to the Upstox API and returns the response as a string
        /// </summary>
        /// <param name="endpoint">The API endpoint URL</param>
        /// <returns>JSON response as string, or null if the request fails</returns>
        protected async Task<string?> GetApiResponseAsync(string endpoint)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, endpoint);
                request.Headers.Add("Accept", "application/json");
                request.Headers.Add("Authorization", $"Bearer {BearerToken}");

                var response = await HttpClient.SendAsync(request);
                response.EnsureSuccessStatusCode();

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"API request failed for {endpoint}: {ex.Message}");
                return null;
            }
        }

        /// <summary>
        /// Safely parses a JSON element value to a decimal, handling various formats.
        /// </summary>
        /// <param name="element">The JSON element to parse</param>
        /// <returns>Parsed decimal value, or 0 if parsing fails</returns>
        protected decimal TryParseDecimalValue(JsonElement element)
        {
            try
            {
                // Try direct decimal parsing if it's a number
                if (element.ValueKind == JsonValueKind.Number)
                {
                    return element.GetDecimal();
                }

                // Try string parsing if it's a string representation of a number
                if (element.ValueKind == JsonValueKind.String)
                {
                    string strValue = element.GetString() ?? string.Empty;
                    if (decimal.TryParse(strValue, out decimal result))
                    {
                        return result;
                    }
                }

                // Return 0 for null or other types
                return 0m;
            }
            catch (FormatException)
            {
                Console.WriteLine($"Failed to parse decimal value: {element}. Defaulting to 0.");
                return 0m;
            }
        }
    }
}
