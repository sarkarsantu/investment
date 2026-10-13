using InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;
using InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching company profile data from Upstox API
    /// </summary>
    public class UpstoxCompanyProfileService : UpstoxApiServiceBase, IUpstoxCompanyProfileService
    {
        public UpstoxCompanyProfileService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves company profile information for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Company profile response containing company description, sector, and market cap</returns>
        public async Task<CompanyProfileResponse> GetCompanyProfileAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/profile";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new CompanyProfileResponse { Status = "error" };

                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new CompanyProfileResponse { Status = "error" };

                        var response = new CompanyProfileResponse { Status = "success" };
                        var data = new Model.Upstox.Fundamental.CompanyProfile.Data();

                        if (root.TryGetProperty("data", out var dataElement))
                        {
                            if (dataElement.TryGetProperty("company_profile", out var companyProfileElem))
                                data.CompanyProfile = companyProfileElem.GetString() ?? string.Empty;

                            if (dataElement.TryGetProperty("sector", out var sectorElem))
                                data.Sector = sectorElem.GetString() ?? string.Empty;

                            // Parse sector_market_cap_inr
                            if (dataElement.TryGetProperty("sector_market_cap_inr", out var inrElem))
                            {
                                var inrData = new MarketCapInfo();
                                if (inrElem.TryGetProperty("value", out var valueElem))
                                    inrData.Value = TryParseDecimalValue(valueElem);
                                if (inrElem.TryGetProperty("unit", out var unitElem))
                                    inrData.Unit = unitElem.GetString() ?? string.Empty;
                                if (inrElem.TryGetProperty("formatted", out var formattedElem))
                                    inrData.Formatted = formattedElem.GetString() ?? string.Empty;
                                data.SectorMarketCapInr = inrData;
                            }

                            // Parse sector_market_cap_usd
                            if (dataElement.TryGetProperty("sector_market_cap_usd", out var usdElem))
                            {
                                var usdData = new MarketCapInfo();
                                if (usdElem.TryGetProperty("value", out var valueElem))
                                    usdData.Value = TryParseDecimalValue(valueElem);
                                if (usdElem.TryGetProperty("unit", out var unitElem))
                                    usdData.Unit = unitElem.GetString() ?? string.Empty;
                                if (usdElem.TryGetProperty("formatted", out var formattedElem))
                                    usdData.Formatted = formattedElem.GetString() ?? string.Empty;
                                data.SectorMarketCapUsd = usdData;
                            }
                        }

                        response.Data = data;
                        return response;
                    }
                }

                return new CompanyProfileResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching company profile for ISIN {isin}: {ex.Message}");
                return new CompanyProfileResponse { Status = "error" };
            }
        }
    }
}
