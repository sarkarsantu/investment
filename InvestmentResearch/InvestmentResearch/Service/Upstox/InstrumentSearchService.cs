using InvestmentResearch.Model.Upstox.Fundamental.InstrumentSearch;
using System.Text.Json;

namespace InvestmentResearch.Service.Upstox;

public class InstrumentSearchService : UpstoxApiServiceBase, IInstrumentSearchService
{
    public InstrumentSearchService(HttpClient httpClient) : base(httpClient)
    {
    }

    public async Task<InstrumentSearchResponse> GetCompanyISIN(string nameFromOutsource)
    {
        try
        {
            if(string.IsNullOrWhiteSpace(nameFromOutsource))
                return new InstrumentSearchResponse { Status = "error" };

            string endpoint = $"{ApiBaseUrl}/instruments/search?query={nameFromOutsource}&segment=EQ";
            var jsonResponse = await GetApiResponseAsync(endpoint);

            if (jsonResponse == null)
                return new InstrumentSearchResponse { Status = "error" };
            var instrumentSearchResponse = JsonSerializer.Deserialize<InstrumentSearchResponse>(jsonResponse);
            
            return instrumentSearchResponse ?? new InstrumentSearchResponse { Status = "error" };
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error fetching Instrument Search for query {nameFromOutsource}: {ex.Message}");
            return new InstrumentSearchResponse { Status = "error" };
        }
    }
}
