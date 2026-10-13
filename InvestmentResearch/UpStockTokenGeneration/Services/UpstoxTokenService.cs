using System.Text;
using System.Text.Json;
using UpStockTokenGeneration.Models;

namespace UpStockTokenGeneration.Services;

public class UpstoxTokenService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiUrl = "https://api.upstox.com/v2/login/authorization/token";

    public UpstoxTokenService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UpstoxTokenResponse> GetTokenAsync(
        string code,
        string clientId,
        string clientSecret,
        string redirectUri)
    {
        try
        {
            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "code", code },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "redirect_uri", redirectUri },
                { "grant_type", "authorization_code" }
            });

            var response = await _httpClient.PostAsync(_apiUrl, content);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
            var tokenResponse = JsonSerializer.Deserialize<UpstoxTokenResponse>(responseContent, options);

            return tokenResponse;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"Error calling Upstox API: {ex.Message}");
            throw;
        }
    }
}
