using InvestmentResearch.Model.Upstox.IPO;
using System.Xml;
using System.Xml.Linq;

namespace InvestmentResearch.Service
{
    public interface IIPOStorageService
    {
        Task SaveIPOsAsync(List<IPOData> ipos);
        Task<List<IPOData>> GetNewIPOsAsync(List<IPOData> currentIPOs);
    }

    public class IPOStorageService : IIPOStorageService
    {
        private const string OutputFolder = "Output";
        private const string IPOFileName = "ipos.xml";
        private readonly string _ipoFilePath;

        public IPOStorageService()
        {
            if (!Directory.Exists(OutputFolder))
            {
                Directory.CreateDirectory(OutputFolder);
            }

            _ipoFilePath = Path.Combine(OutputFolder, IPOFileName);
        }

        public async Task SaveIPOsAsync(List<IPOData> ipos)
        {
            try
            {
                XDocument doc;

                // Load existing XML or create new one
                if (File.Exists(_ipoFilePath))
                {
                    doc = XDocument.Load(_ipoFilePath);
                }
                else
                {
                    doc = new XDocument(new XElement("IPOs"));
                }

                var root = doc.Root ?? new XElement("IPOs");

                foreach (var ipo in ipos)
                {
                    // Check if IPO already exists (by ID)
                    var existingIPO = root.Elements("IPO").FirstOrDefault(x => x.Element("Id")?.Value == ipo.Id);

                    if (existingIPO != null)
                    {
                        // Update existing IPO
                        UpdateIPOElement(existingIPO, ipo);
                    }
                    else
                    {
                        // Add new IPO
                        var ipoElement = CreateIPOElement(ipo);
                        root.Add(ipoElement);
                    }
                }

                // Recreate the document with updated root if needed
                if (doc.Root != root)
                {
                    doc = new XDocument(root);
                }

                await Task.Run(() => doc.Save(_ipoFilePath));
                Console.WriteLine($"IPOs saved to: {_ipoFilePath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving IPOs to XML: {ex.Message}");
            }
        }

        public async Task<List<IPOData>> GetNewIPOsAsync(List<IPOData> currentIPOs)
        {
            try
            {
                var newIPOs = new List<IPOData>();

                if (!File.Exists(_ipoFilePath))
                {
                    // No previous data, all are new
                    return currentIPOs;
                }

                XDocument doc = await Task.Run(() => XDocument.Load(_ipoFilePath));
                var root = doc.Root;

                if (root == null)
                {
                    return currentIPOs;
                }

                string today = DateTime.Now.ToString("yyyy-MM-dd");

                foreach (var ipo in currentIPOs)
                {
                    var existingIPO = root.Elements("IPO").FirstOrDefault(x => x.Element("Id")?.Value == ipo.Id);

                    if (existingIPO == null)
                    {
                        // Completely new IPO
                        newIPOs.Add(ipo);
                    }
                    else
                    {
                        // Check if it was saved today
                        var lastSavedDateElement = existingIPO.Element("LastSavedDate");
                        if (lastSavedDateElement == null || lastSavedDateElement.Value != today)
                        {
                            newIPOs.Add(ipo);
                        }
                    }
                }

                return newIPOs;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading IPOs from XML: {ex.Message}");
                return currentIPOs;
            }
        }

        private XElement CreateIPOElement(IPOData ipo)
        {
            return new XElement("IPO",
                new XElement("Id", ipo.Id),
                new XElement("Symbol", ipo.Symbol),
                new XElement("Name", ipo.Name),
                new XElement("Status", ipo.Status),
                new XElement("Isin", ipo.Isin),
                new XElement("IssueType", ipo.IssueType),
                new XElement("IssueSize", ipo.IssueSize),
                new XElement("Industry", ipo.Industry),
                new XElement("MinimumPrice", ipo.MinimumPrice),
                new XElement("MaximumPrice", ipo.MaximumPrice),
                new XElement("BiddingStartDate", ipo.BiddingStartDate),
                new XElement("BiddingEndDate", ipo.BiddingEndDate),
                new XElement("TotalSubscription", ipo.TotalSubscription),
                new XElement("LastSavedDate", DateTime.Now.ToString("yyyy-MM-dd")),
                new XElement("SavedDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            );
        }

        private void UpdateIPOElement(XElement ipoElement, IPOData ipo)
        {
            ipoElement.SetElementValue("Symbol", ipo.Symbol);
            ipoElement.SetElementValue("Name", ipo.Name);
            ipoElement.SetElementValue("Status", ipo.Status);
            ipoElement.SetElementValue("Isin", ipo.Isin);
            ipoElement.SetElementValue("IssueType", ipo.IssueType);
            ipoElement.SetElementValue("IssueSize", ipo.IssueSize);
            ipoElement.SetElementValue("Industry", ipo.Industry);
            ipoElement.SetElementValue("MinimumPrice", ipo.MinimumPrice);
            ipoElement.SetElementValue("MaximumPrice", ipo.MaximumPrice);
            ipoElement.SetElementValue("BiddingStartDate", ipo.BiddingStartDate);
            ipoElement.SetElementValue("BiddingEndDate", ipo.BiddingEndDate);
            ipoElement.SetElementValue("TotalSubscription", ipo.TotalSubscription);
            ipoElement.SetElementValue("LastSavedDate", DateTime.Now.ToString("yyyy-MM-dd"));
            ipoElement.SetElementValue("SavedDateTime", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
