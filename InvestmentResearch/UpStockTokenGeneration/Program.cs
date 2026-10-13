// See https://aka.ms/new-console-template for more information
using UpStockTokenGeneration.Services;

// Configuration
const string code = "MtYdjy";
const string clientId = "3dc5e5a9-244d-44eb-ba66-eb57e5d5d4fd";
const string clientSecret = "zb6kyxi4ac";
const string redirectUri = "https://127.0.0.1:8080/callback";
//https://api.upstox.com/v2/login/authorization/dialog?client_id=3dc5e5a9-244d-44eb-ba66-eb57e5d5d4fd&redirect_uri=https://127.0.0.1:8080/callback&response_type=code
/*
SELECT * 
FROM [dbo].[tblCompanyResearch] 
WHERE CAST(CreateDate AS DATE) = CAST(GETDATE() AS DATE) 
   OR CAST(ModifyDate AS DATE) = CAST(GETDATE() AS DATE);
*/
// Create HttpClient
using var httpClient = new HttpClient();

// Create service and get token
var tokenService = new UpstoxTokenService(httpClient);

try
{
    Console.WriteLine("Calling Upstox Get Token API...\n");
    var tokenResponse = await tokenService.GetTokenAsync(code, clientId, clientSecret, redirectUri);

    if (tokenResponse != null)
    {
        Console.WriteLine("✓ Token Retrieved Successfully!\n");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("USER INFORMATION");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine($"Email:     {tokenResponse.Email}");
        Console.WriteLine($"User ID:   {tokenResponse.UserId}");
        Console.WriteLine($"User Name: {tokenResponse.UserName}");
        Console.WriteLine($"Broker:    {tokenResponse.Broker}");
        Console.WriteLine($"User Type: {tokenResponse.UserType}");
        Console.WriteLine($"Active:    {tokenResponse.IsActive}");
        Console.WriteLine();

        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("EXCHANGES & PRODUCTS");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine($"Exchanges: {string.Join(", ", tokenResponse.Exchanges)}");
        Console.WriteLine($"Products:  {string.Join(", ", tokenResponse.Products)}");
        Console.WriteLine($"Order Types: {string.Join(", ", tokenResponse.OrderTypes)}");
        Console.WriteLine();

        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("TOKENS - COPY THESE VALUES");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine($"\nAccess Token:\n{tokenResponse.AccessToken}\n");
        Console.WriteLine($"Extended Token:\n{tokenResponse.ExtendedToken}\n");
        Console.WriteLine("=".PadRight(80, '='));
        Console.WriteLine("\nTokens displayed above. Press any key to exit...");
        Console.ReadKey();
    }
}
catch (Exception ex)
{
    Console.WriteLine($"✗ Error: {ex.Message}");
    Console.WriteLine("\nPress any key to exit...");
    Console.ReadKey();
}
