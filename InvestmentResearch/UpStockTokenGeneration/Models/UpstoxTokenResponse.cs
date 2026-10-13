using System.Text.Json.Serialization;

namespace UpStockTokenGeneration.Models;

public class UpstoxTokenResponse
{
    [JsonPropertyName("email")]
    public string Email { get; set; }

    [JsonPropertyName("exchanges")]
    public List<string> Exchanges { get; set; }

    [JsonPropertyName("products")]
    public List<string> Products { get; set; }

    [JsonPropertyName("broker")]
    public string Broker { get; set; }

    [JsonPropertyName("user_id")]
    public string UserId { get; set; }

    [JsonPropertyName("user_name")]
    public string UserName { get; set; }

    [JsonPropertyName("order_types")]
    public List<string> OrderTypes { get; set; }

    [JsonPropertyName("user_type")]
    public string UserType { get; set; }

    [JsonPropertyName("poa")]
    public bool Poa { get; set; }

    [JsonPropertyName("is_active")]
    public bool IsActive { get; set; }

    [JsonPropertyName("access_token")]
    public string AccessToken { get; set; }

    [JsonPropertyName("extended_token")]
    public string ExtendedToken { get; set; }
}
