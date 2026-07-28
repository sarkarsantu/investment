using InvestmentResearch.Model;

namespace InvestmentResearch.Service
{
    public class RSSLinkService : IRSSLinkService
    {
        private readonly List<Domain> _domains = new();

        public RSSLinkService()
        {
            int i = 0;
            _domains.Add(new Domain { Name = "scanx.trade", ReadableName = "ScanX Trade", Rank = ++i });
            _domains.Add(new Domain { Name = "livemint", ReadableName = "Live mint", Rank = ++i });
            _domains.Add(new Domain { Name = "moneycontrol", ReadableName = "Money control", Rank = ++i });
            _domains.Add(new Domain { Name = "timesofindia", ReadableName = "Times of India", Rank = ++i });
            _domains.Add(new Domain { Name = "reuters", ReadableName = "Reuters", Rank = ++i });
            _domains.Add(new Domain { Name = "economictimes", ReadableName = "Economic Times", Rank = ++i });
            _domains.Add(new Domain { Name = "cnbc", ReadableName = "CNBC", Rank = ++i });
            _domains.Add(new Domain { Name = "cnbctv18", ReadableName = "CNBC-TV18", Rank = ++i });
            _domains.Add(new Domain { Name = "ddindia.co.in", ReadableName = "DD India", Rank = ++i });
            _domains.Add(new Domain { Name = "thehindubusinessline", ReadableName = "The Hindu Business Line", Rank = ++i });
            _domains.Add(new Domain { Name = "financialexpress", ReadableName = "Financial Express", Rank = ++i });
            _domains.Add(new Domain { Name = "business-standard", ReadableName = "Business Standard", Rank = ++i });
            _domains.Add(new Domain { Name = "bseindia", ReadableName = "BSE India", Rank = ++i });
            _domains.Add(new Domain { Name = "nseindia", ReadableName = "NSE India", Rank = ++i });
            _domains.Add(new Domain { Name = "msn.com", ReadableName = "MSN", Rank = ++i });
            _domains.Add(new Domain { Name = "indiatoday.in", ReadableName = "India Today", Rank = ++i });
            _domains.Add(new Domain { Name = "firstpost.com", ReadableName = "Firstpost", Rank = ++i });
            _domains.Add(new Domain { Name = "defence.in", ReadableName = "Defence India", Rank = ++i });
            _domains.Add(new Domain { Name = "upstox.com", ReadableName = "Upstox", Rank = ++i });
            _domains.Add(new Domain { Name = "equitymaster.com", ReadableName = "Equity Master", Rank = ++i });
            _domains.Add(new Domain { Name = "indiatvnews.com", ReadableName = "India TV News", Rank = ++i });
            _domains.Add(new Domain { Name = "news18.com", ReadableName = "News 18", Rank = ++i });
            _domains.Add(new Domain { Name = "telegraphindia.com", ReadableName = "The Telegraph India", Rank = ++i });
            _domains.Add(new Domain { Name = "rediff.com", ReadableName = "Rediff", Rank = ++i });
            _domains.Add(new Domain { Name = "indianexpress", ReadableName = "Indian Express", Rank = ++i });
            _domains.Add(new Domain { Name = "investmentguruindia.com", ReadableName = "Investment Guru India", Rank = ++i });
            _domains.Add(new Domain { Name = "espn.in", ReadableName = "ESPN India", Rank = ++i });
            _domains.Add(new Domain { Name = "businessworld.in", ReadableName = "Business World", Rank = ++i });
            _domains.Add(new Domain { Name = "ng.investing.com", ReadableName = "Investing.com", Rank = ++i });
            _domains.Add(new Domain { Name = "businesstoday.in", ReadableName = "Business Today", Rank = ++i });
            _domains.Add(new Domain { Name = "hindustantimes.com", ReadableName = "Hindustan Times", Rank = ++i });
            _domains.Add(new Domain { Name = "saurenergy.com", ReadableName = "Saur Energy", Rank = ++i });
            _domains.Add(new Domain { Name = "mondaq.com", ReadableName = "Mondaq", Rank = ++i });
            _domains.Add(new Domain { Name = "indianpsu.com", ReadableName = "Indian PSU", Rank = ++i });
            _domains.Add(new Domain { Name = "bloomberg", ReadableName = "Bloomberg", Rank = ++i });
            _domains.Add(new Domain { Name = "chemindigest.com", ReadableName = "Chemindigest", Rank = ++i });
            _domains.Add(new Domain { Name = "timestech.in", ReadableName = "Times Tech", Rank = ++i });
            _domains.Add(new Domain { Name = "orfonline.org", ReadableName = "ORF Online", Rank = ++i });
            _domains.Add(new Domain { Name = "constructionworld.in", ReadableName = "Construction World", Rank = ++i });
            _domains.Add(new Domain { Name = "policycircle.org", ReadableName = "Policy Circle", Rank = ++i });
            _domains.Add(new Domain { Name = "mercomindia.com", ReadableName = "Mercom India", Rank = ++i });
            _domains.Add(new Domain { Name = "thecore.in", ReadableName = "The Core", Rank = ++i });
            _domains.Add(new Domain { Name = "finance.yahoo.com", ReadableName = "Yahoo Finance", Rank = ++i });
            _domains.Add(new Domain { Name = "utkarsh.com", ReadableName = "Utkarsh", Rank = ++i });
            _domains.Add(new Domain { Name = "egov.eletsonline.com", ReadableName = "Elets eGov", Rank = ++i });
            _domains.Add(new Domain { Name = "constructionweekonline.in", ReadableName = "Construction Week Online", Rank = ++i });
            _domains.Add(new Domain { Name = "indianchemicalnews.com", ReadableName = "Indian Chemical News", Rank = ++i });
            _domains.Add(new Domain { Name = "indiagazette.com", ReadableName = "India Gazette", Rank = ++i });
            _domains.Add(new Domain { Name = "dailyexcelsior.com", ReadableName = "Daily Excelsior", Rank = ++i });
            _domains.Add(new Domain { Name = "tradebrains.in", ReadableName = "Trade Brains", Rank = ++i });
            _domains.Add(new Domain { Name = "devdiscourse.com", ReadableName = "Dev Discourse", Rank = ++i });
            _domains.Add(new Domain { Name = "gktoday.in", ReadableName = "GK Today", Rank = ++i });
            _domains.Add(new Domain { Name = "outlookbusiness.com", ReadableName = "Outlook Business", Rank = ++i });
            _domains.Add(new Domain { Name = "businessoutreach.in", ReadableName = "Business Outreach", Rank = ++i });
            _domains.Add(new Domain { Name = "techcircle.in", ReadableName = "Tech Circle", Rank = ++i });
            _domains.Add(new Domain { Name = "english.punjabkesari.com", ReadableName = "Punjab Kesari", Rank = ++i });
            _domains.Add(new Domain { Name = "electronicsforyou.biz", ReadableName = "Electronics For You", Rank = ++i });
            _domains.Add(new Domain { Name = "in.investing.com", ReadableName = "Investing.com India", Rank = ++i });
            _domains.Add(new Domain { Name = "asiabusinessoutlook.com", ReadableName = "Asia Business Outlook", Rank = ++i });
            _domains.Add(new Domain { Name = "insights.dsij.in", ReadableName = "DSIJ Insights", Rank = ++i });
            _domains.Add(new Domain { Name = "swarajyamag.com", ReadableName = "Swarajya Mag", Rank = ++i });
            _domains.Add(new Domain { Name = "indianinfrastructure.com", ReadableName = "Indian Infrastructure", Rank = ++i });
            _domains.Add(new Domain { Name = "bharatshakti.in", ReadableName = "Bharat Shakti", Rank = ++i });
            _domains.Add(new Domain { Name = "cxotoday.com", ReadableName = "CXO Today", Rank = ++i });
            _domains.Add(new Domain { Name = "pib.gov.in", ReadableName = "Press Information Bureau (PIB)", Rank = ++i });
            _domains.Add(new Domain { Name = "manufacturingtodayindia.com", ReadableName = "Manufacturing Today India", Rank = ++i });
            _domains.Add(new Domain { Name = "staffnews.in", ReadableName = "Staff News India", Rank = ++i });
            _domains.Add(new Domain { Name = "therightnews.in", ReadableName = "The Right News", Rank = ++i });
            _domains.Add(new Domain { Name = "newsbytesapp.com", ReadableName = "News Bytes", Rank = ++i });
            _domains.Add(new Domain { Name = "bisinfotech.com", ReadableName = "BIS Infotech", Rank = ++i });
            _domains.Add(new Domain { Name = "theweek.in", ReadableName = "The Week India", Rank = ++i });
            _domains.Add(new Domain { Name = "newsonair.gov.in", ReadableName = "News on AIR (All India Radio)", Rank = ++i });
            _domains.Add(new Domain { Name = "aninews.in", ReadableName = "ANI News", Rank = ++i });
            _domains.Add(new Domain { Name = "equitypandit.com", ReadableName = "Equity Pandit", Rank = ++i });
            _domains.Add(new Domain { Name = "smefutures.com", ReadableName = "SME Futures", Rank = ++i });
            _domains.Add(new Domain { Name = "tennews.in", ReadableName = "Ten News India", Rank = ++i });
            _domains.Add(new Domain { Name = "fortuneindia.com", ReadableName = "Fortune India", Rank = ++i });
            _domains.Add(new Domain { Name = "indianweb2.com", ReadableName = "Indian Web 2.0", Rank = ++i });
            _domains.Add(new Domain { Name = "ommcomnews.com", ReadableName = "Omm com news", Rank = ++i });
            _domains.Add(new Domain { Name = "autopunditz.com", ReadableName = "Auto pund itz", Rank = ++i });
            _domains.Add(new Domain { Name = "urbanacres.in", ReadableName = "Urban Acres", Rank = ++i });
            _domains.Add(new Domain { Name = "thehansindia.com", ReadableName = "The Hans India", Rank = ++i });
            _domains.Add(new Domain { Name = "english.hindusthansamachar.in", ReadableName = "Hindustan Samachar", Rank = ++i });
            _domains.Add(new Domain { Name = "m.dailyhunt.in", ReadableName = "Daily Hunt", Rank = ++i });
            _domains.Add(new Domain { Name = "thestatesman.com", ReadableName = "The Statesman", Rank = ++i });
            _domains.Add(new Domain { Name = "english.mathrubhumi.com", ReadableName = "Mathrubhumi", Rank = ++i });
            _domains.Add(new Domain { Name = "unlistedzone.com", ReadableName = "Unlisted Zone", Rank = ++i });
            _domains.Add(new Domain { Name = "tathya.in", ReadableName = "Tathya", Rank = ++i });
            _domains.Add(new Domain { Name = "downtoearth.org.in", ReadableName = "Down to Earth", Rank = ++i });
            _domains.Add(new Domain { Name = "odishabytes.com", ReadableName = "Odisha Bytes", Rank = ++i });
            _domains.Add(new Domain { Name = "orissapost.com", ReadableName = "Orissa Post", Rank = ++i });
            _domains.Add(new Domain { Name = "uniindia.com", ReadableName = "UNI India", Rank = ++i });
            _domains.Add(new Domain { Name = "voice.lapaas.com", ReadableName = "Lapaas Voice", Rank = ++i });
            _domains.Add(new Domain { Name = "dtnext.in", ReadableName = "DT Next", Rank = ++i });
            _domains.Add(new Domain { Name = "barandbench.com", ReadableName = "Bar & Bench", Rank = ++i });
            _domains.Add(new Domain { Name = "thewire.in", ReadableName = "The Wire", Rank = ++i });
            _domains.Add(new Domain { Name = "whalesbook.com", ReadableName = "Whales Book", Rank = ++i });
            _domains.Add(new Domain { Name = "psuconnect.in", ReadableName = "PSU Connect", Rank = ++i });
            _domains.Add(new Domain { Name = "theprint.in", ReadableName = "The Print", Rank = ++i });
            _domains.Add(new Domain { Name = "awazthevoice.in", ReadableName = "Awaz The Voice", Rank = ++i });
            _domains.Add(new Domain { Name = "analyticsindiamag.com", ReadableName = "Analytics India Mag", Rank = ++i });
            _domains.Add(new Domain { Name = "niftytrader.in", ReadableName = "Nifty Trader", Rank = ++i });
            _domains.Add(new Domain { Name = "communicationstoday.co.in", ReadableName = "Communications Today", Rank = ++i });
            _domains.Add(new Domain { Name = "opindia.com", ReadableName = "Op India", Rank = ++i });
            _domains.Add(new Domain { Name = "timeskuwait.com", ReadableName = "Times Kuwait", Rank = ++i });
            _domains.Add(new Domain { Name = "peoplematters.in", ReadableName = "People Matters", Rank = ++i });
            _domains.Add(new Domain { Name = "freepressjournal.in", ReadableName = "Free Press Journal", Rank = ++i });
            _domains.Add(new Domain { Name = "deccanchronicle.com", ReadableName = "Deccan Chronicle", Rank = ++i });
            _domains.Add(new Domain { Name = "livelaw.in", ReadableName = "Live Law", Rank = ++i });
            _domains.Add(new Domain { Name = "siasat.com", ReadableName = "Siasat", Rank = ++i });
            _domains.Add(new Domain { Name = "thehawk.in", ReadableName = "The Hawk", Rank = ++i });
            _domains.Add(new Domain { Name = "powerpeakdigest.com", ReadableName = "Power Peak Digest", Rank = ++i });
            _domains.Add(new Domain { Name = "ascendants.in", ReadableName = "Ascendants", Rank = ++i });
            _domains.Add(new Domain { Name = "pragativadi.com", ReadableName = "Pragativadi", Rank = ++i });
            _domains.Add(new Domain { Name = "marketsmojo.com", ReadableName = "Markets Mojo", Rank = ++i });
            _domains.Add(new Domain { Name = "taxscan.in", ReadableName = "Tax Scan", Rank = ++i });
            _domains.Add(new Domain { Name = "ceoinsightsasia.com", ReadableName = "CEO Insights Asia", Rank = ++i });
            _domains.Add(new Domain { Name = "insightsonindia.com", ReadableName = "Insights on India", Rank = ++i });
            _domains.Add(new Domain { Name = "thenewsnow.co.in", ReadableName = "The News Now", Rank = ++i });
            _domains.Add(new Domain { Name = "thekashmirhorizon.com", ReadableName = "The Kashmir Horizon", Rank = ++i });
            _domains.Add(new Domain { Name = "kashmirlife.net", ReadableName = "Kashmir Life", Rank = ++i });
            _domains.Add(new Domain { Name = "bioenergytimes.com", ReadableName = "Bio Energy Times", Rank = ++i });
            _domains.Add(new Domain { Name = "greentechlead.com", ReadableName = "Green Tech Lead", Rank = ++i });
            _domains.Add(new Domain { Name = "renewablewatch.in", ReadableName = "Renewable Watch", Rank = ++i });
            _domains.Add(new Domain { Name = "keralakaumudi.com", ReadableName = "Kerala Kaumudi", Rank = ++i });
            _domains.Add(new Domain { Name = "propnewstime.com", ReadableName = "Prop News Time", Rank = ++i });
            _domains.Add(new Domain { Name = "techstory.in", ReadableName = "Tech Story", Rank = ++i });
            _domains.Add(new Domain { Name = "morungexpress.com", ReadableName = "Morung Express", Rank = ++i });
            _domains.Add(new Domain { Name = "hindupost.in", ReadableName = "Hindu Post", Rank = ++i });
            _domains.Add(new Domain { Name = "telanganatoday.com", ReadableName = "Telangana Today", Rank = ++i });
            _domains.Add(new Domain { Name = "thenewsmill.com", ReadableName = "The News Mill", Rank = ++i });
            _domains.Add(new Domain { Name = "knnindia.co.in", ReadableName = "KNN India", Rank = ++i });
            _domains.Add(new Domain { Name = "easternmirrornagaland.com", ReadableName = "Eastern Mirror Nagaland", Rank = ++i });
            _domains.Add(new Domain { Name = "english.newstrack.com", ReadableName = "News Track", Rank = ++i });
            _domains.Add(new Domain { Name = "news.ventureintelligence.com", ReadableName = "Venture Intelligence", Rank = ++i });
            _domains.Add(new Domain { Name = "indiainfoline.com", ReadableName = "India Infoline", Rank = ++i });
            _domains.Add(new Domain { Name = "millenniumpost.in", ReadableName = "Millennium Post", Rank = ++i });
            _domains.Add(new Domain { Name = "telecomlead.com", ReadableName = "Telecom Lead", Rank = ++i });
            _domains.Add(new Domain { Name = "cobrapost.com", ReadableName = "Cobrapost", Rank = ++i });
            _domains.Add(new Domain { Name = "lawbeat.in", ReadableName = "Law Beat", Rank = ++i });
            
            _domains.Add(new Domain { Name = "zeebiz.com", ReadableName = "Zee Biz", Rank = ++i });
            _domains.Add(new Domain { Name = "analyticsinsight.net", ReadableName = "Analytics Insight", Rank = ++i });
        }

        /// <summary>
        /// Extracts the domain name from the Href property.
        /// Examples: "https://timesofindia.com/..." → "Times of India"
        /// </summary>
        public string ExtractDomain(string href)
        {
            ArgumentNullException.ThrowIfNull(href);
            if (string.IsNullOrEmpty(href))
            {
                return "Unknown";
            }

            try
            {
                var uri = new Uri(href);
                var hostParts = uri.Host.Split('.');

                var readableDomain = _domains.FirstOrDefault(d => href.IndexOf(d.Name, StringComparison.OrdinalIgnoreCase) > -1);

                if (readableDomain != null)
                {
                    return readableDomain.ReadableName;
                }

                // Get the domain name (second-to-last part before TLD)
                if (hostParts.Length >= 2)
                {
                    string domainName = hostParts[^2]; // Get second-to-last part

                    return "Unknown" + System.Text.RegularExpressions.Regex.Replace(
                            domainName,
                            "(?<!^)(?=[A-Z])",
                            " "
                        );
                }
                else
                {
                    return "Unknown" + uri.Host;
                }
            }
            catch
            {
                return "Unknown";
            }
        }

        public List<Link> UpdateLinksBySLNo(List<Link> links)
        {
            var ret = new List<Link>();
            foreach (var link in links)
            {
                var readableDomain = _domains.FirstOrDefault(d => d.ReadableName == link.Domain);

                if (readableDomain != null)
                {
                    link.SlNo = readableDomain.Rank;
                }
                else
                {
                    link.SlNo = _domains.Count + 1; // Unknown domain
                }
                ret.Add(link);
            }

            return ret;
        }
    }
}
