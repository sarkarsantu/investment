using InvestmentResearch.Model.Upstox.IPO;
using InvestmentResearch.Service.Upstox;

namespace InvestmentResearch.Service
{
    public interface IIPOProcessorService
    {
        Task ProcessOpenIPOsAsync();
    }

    public class IPOProcessorService : IIPOProcessorService
    {
        private readonly IUpstoxIPOService _upstoxIPOService;
        private readonly IIPOStorageService _ipoStorageService;
        private readonly IUpstoxFundamentalsService _upstoxFundamentalsService;
        private readonly ICompanyResearchService _companyResearchService;

        public IPOProcessorService(
            IUpstoxIPOService upstoxIPOService, 
            IIPOStorageService ipoStorageService,
            IUpstoxFundamentalsService upstoxFundamentalsService,
            ICompanyResearchService companyResearchService)
        {
            _upstoxIPOService = upstoxIPOService;
            _ipoStorageService = ipoStorageService;
            _upstoxFundamentalsService = upstoxFundamentalsService;
            _companyResearchService = companyResearchService;
        }

        public async Task ProcessOpenIPOsAsync()
        {
            try
            {
                // Get IPOs from Upstox API
                var allOpenIPOs = await _upstoxIPOService.GetOpenIPOsAsync();

                if (allOpenIPOs == null || allOpenIPOs.Count == 0)
                {
                    Console.WriteLine("No open IPOs found.");
                    return;
                }

                foreach (var ipo in allOpenIPOs)
                {
                    await _companyResearchService.RunReachModel(ipo.Isin, ipo.Name, ipo.Symbol);
                }
                
                /*
                // Get only IPOs that were not saved yesterday
                var newIPOs = await _ipoStorageService.GetNewIPOsAsync(allOpenIPOs);

                if (newIPOs == null || newIPOs.Count == 0)
                {
                    Console.WriteLine("No new IPOs to process (all IPOs were already saved today).");
                }
                else
                {
                    Console.WriteLine($"Found {newIPOs.Count} new or updated IPOs.");
                    foreach (var ipo in newIPOs)
                    {
                        Console.WriteLine($"  - {ipo.Symbol}: {ipo.Name} ({ipo.Industry})");
                    }

                    // Fetch fundamentals for each new IPO
                    await FetchFundamentalsForIPOsAsync(newIPOs);
                }

                // Save all IPOs to XML storage
                await _ipoStorageService.SaveIPOsAsync(allOpenIPOs);
                */
                Console.WriteLine($"{TimeOnly.FromDateTime(DateTime.Now).ToString("hh:mm tt")} ## IPO processing completed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing IPOs: {ex.Message}");
            }
        }

        private async Task FetchFundamentalsForIPOsAsync(List<IPOData> ipos)
        {
            Console.WriteLine("\nFetching fundamentals data for open IPOs...");

            foreach (var ipo in ipos)
            {
                if (string.IsNullOrEmpty(ipo.Isin))
                {
                    Console.WriteLine($"Skipping {ipo.Symbol}: ISIN not available");
                    continue;
                }

                try
                {
                    var fundamentalsData = await _upstoxFundamentalsService.GetAllFundamentalsAsync(
                        ipo.Isin,
                        ipo.Symbol,
                        ipo.Name);

                    // You can now use fundamentalsData for further processing
                    // For example: save to database, log key metrics, etc.
                    if (fundamentalsData.FetchStatus == "success")
                    {
                        Console.WriteLine($"? Fundamentals fetched successfully for {ipo.Symbol}");
                    }
                    else if (fundamentalsData.FetchStatus == "partial")
                    {
                        Console.WriteLine($"? Partial data fetched for {ipo.Symbol}");
                    }
                    else
                    {
                        Console.WriteLine($"? Failed to fetch fundamentals for {ipo.Symbol}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error fetching fundamentals for {ipo.Symbol}: {ex.Message}");
                }
            }

            Console.WriteLine("Fundamentals fetch completed.\n");
        }
    }
}
