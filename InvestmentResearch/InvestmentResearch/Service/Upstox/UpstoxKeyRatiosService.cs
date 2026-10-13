using InvestmentResearch.Model.Upstox.Fundamental.KeyRatios;
using System.Text.Json;
using System.Xml.Linq;

namespace InvestmentResearch.Service.Upstox
{
    /// <summary>
    /// Service for fetching key financial ratios from Upstox API
    /// </summary>
    public class UpstoxKeyRatiosService : UpstoxApiServiceBase, IUpstoxKeyRatiosService
    {
        public UpstoxKeyRatiosService(HttpClient httpClient) : base(httpClient)
        {
        }

        /// <summary>
        /// Retrieves key financial ratios for a given ISIN
        /// </summary>
        /// <param name="isin">The ISIN code of the security</param>
        /// <returns>Key ratios response containing P/E, P/B, ROA, ROE, ROCE, and EV/EBITDA ratios</returns>
        public async Task<KeyRatiosResponse> GetKeyRatiosAsync(string isin)
        {
            try
            {
                string endpoint = $"{ApiBaseUrl}/fundamentals/{isin}/key-ratios";
                var jsonResponse = await GetApiResponseAsync(endpoint);

                if (jsonResponse == null)
                    return new KeyRatiosResponse { Status = "error" };

                // Manual JSON parsing to extract key ratios data
                using (JsonDocument doc = JsonDocument.Parse(jsonResponse))
                {
                    var root = doc.RootElement;

                    if (root.TryGetProperty("status", out var statusElement))
                    {
                        string status = statusElement.GetString();
                        if (status != "success")
                            return new KeyRatiosResponse { Status = "error" };

                        var response = new KeyRatiosResponse { Status = "success", Data = new Model.Upstox.Fundamental.KeyRatios.Data() };

                        if (root.TryGetProperty("data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Array)
                        {
                            foreach (var item in dataElement.EnumerateArray())
                            {
                                string Name = item.TryGetProperty("name", out var nameElem) ? nameElem.GetString() : string.Empty;
                                switch (Name?.ToLower())
                                {
                                    case "p/e":
                                        {
                                            response.Data.PE = new PE
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                        
                                        case "p/b":
                                        {
                                            response.Data.PB = new PB
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                        case "roa":
                                        {
                                            response.Data.ROA = new ROA
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                        case "roe":
                                        {
                                            response.Data.ROE = new ROE
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                        case "roce":
                                        {
                                            response.Data.ROCE = new ROCE
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                    case "quick ratio":
                                        {
                                            response.Data.QuickRatio = new QuickRatio
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                    case "ev/ebitda":
                                        {
                                            response.Data.EBITDA = new EBITDA
                                            {
                                                CompanyValue = item.TryGetProperty("company_value", out var companyElem) ? companyElem.GetString() : string.Empty,
                                                SectorValue = item.TryGetProperty("sector_value", out var sectorElem) ? sectorElem.GetString() : string.Empty
                                            };
                                            break;
                                        }
                                        default:
                                        {
                                            // Handle any other key ratios if needed
                                            Console.WriteLine($"Unhandled key ratio: {Name}");
                                            throw new NotImplementedException($"Key ratio '{Name}' is not implemented in the response model.");
                                        }

                                }
                            }
                        }

                        return response;
                    }
                }

                return new KeyRatiosResponse { Status = "error" };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching key ratios for ISIN {isin}: {ex.Message}");
                return new KeyRatiosResponse { Status = "error" };
            }
        }
    }
}
