using Humanizer;
using InvestmentResearch.Model;
using Mscc.GenerativeAI.Types;
using System.Security.Cryptography;

namespace InvestmentResearch.Service
{
    public class RSSLinkService : IRSSLinkService
    {
        private readonly List<Domain> _domains = new();

        public RSSLinkService()
        {
            int i = 0;
            _domains.Add(new Domain { Name = "scanx.trade", ReadableName = "ScanX Trade", Rank = ++i });
            _domains.Add(new Domain { Name = "livemint.com", ReadableName = "Live mint", Rank = ++i });
            _domains.Add(new Domain { Name = "moneycontrol.com", ReadableName = "Money control", Rank = ++i });
            _domains.Add(new Domain { Name = "timesofindia.com", ReadableName = "Times of India", Rank = ++i });
            _domains.Add(new Domain { Name = "narendramodi.in", ReadableName = "Narendra Modi", Rank = ++i });
            _domains.Add(new Domain { Name = "reuters.com", ReadableName = "Reuters", Rank = ++i });
            _domains.Add(new Domain { Name = "economictimes.com", ReadableName = "Economic Times", Rank = ++i });
            _domains.Add(new Domain { Name = "ndtvprofit.com", ReadableName = "NDTV Profit", Rank = ++i });
            _domains.Add(new Domain { Name = "cnbc.com", ReadableName = "CNBC", Rank = ++i });
            _domains.Add(new Domain { Name = "cnbctv18.com", ReadableName = "CNBC-TV18", Rank = ++i });
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
            _domains.Add(new Domain { Name = "chemicals.economictimes.indiatimes.com", ReadableName = "Economic Times - Chemicals", Rank = ++i });
            _domains.Add(new Domain { Name = "economictimes.indiatimes.com", ReadableName = "Economic Times", Rank = ++i });
            _domains.Add(new Domain { Name = "auto.economictimes.indiatimes.com", ReadableName = "Economic Times - Auto", Rank = ++i });
            _domains.Add(new Domain { Name = "cio.economictimes.indiatimes.com", ReadableName = "Economic Times - CIO", Rank = ++i });
            _domains.Add(new Domain { Name = "manufacturing.economictimes.indiatimes.com", ReadableName = "Economic Times - Manufacturing", Rank = ++i });
            _domains.Add(new Domain { Name = "energy.economictimes.indiatimes.com", ReadableName = "Economic Times - Energy", Rank = ++i });
            _domains.Add(new Domain { Name = "timesofindia.indiatimes.com", ReadableName = "Times of India", Rank = ++i });
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
            _domains.Add(new Domain { Name = "ddnews.gov.in", ReadableName = "DD News", Rank = ++i });
            _domains.Add(new Domain { Name = "saurenergy.com", ReadableName = "Saur Energy", Rank = ++i });
            _domains.Add(new Domain { Name = "youtube.com", ReadableName = "YouTube", Rank = ++i });
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
            _domains.Add(new Domain { Name = "sahi.com", ReadableName = "Sahi", Rank = ++i });
            _domains.Add(new Domain { Name = "goodreturns.in", ReadableName = "Good Returns", Rank = ++i });
            _domains.Add(new Domain { Name = "informistmedia.com", ReadableName = "Informist Media", Rank = ++i });
            _domains.Add(new Domain { Name = "smestreet.in", ReadableName = "SME Street", Rank = ++i });
            _domains.Add(new Domain { Name = "frontierindia.com", ReadableName = "Frontier India", Rank = ++i });
            _domains.Add(new Domain { Name = "pharmabiz.com", ReadableName = "Pharma Biz", Rank = ++i });
            _domains.Add(new Domain { Name = "the - aiff.com", ReadableName = "The AIFF", Rank = ++i });
            _domains.Add(new Domain { Name = "defence.newsd.in", ReadableName = "Defence News D", Rank = ++i });
            _domains.Add(new Domain { Name = "currentaffairs.adda247.com", ReadableName = "Current Affairs Adda247", Rank = ++i });
            _domains.Add(new Domain { Name = "frontline.thehindu.com", ReadableName = "Frontline The Hindu", Rank = ++i });
            _domains.Add(new Domain { Name = "alchempro.com", ReadableName = "Alchempro", Rank = ++i });
            _domains.Add(new Domain { Name = "kashmirvision.in", ReadableName = "Kashmir Vision", Rank = ++i });
            _domains.Add(new Domain { Name = "lovin.co", ReadableName = "Lovin", Rank = ++i });
            _domains.Add(new Domain { Name = "knocksense.com", ReadableName = "Knocksense", Rank = ++i });
            _domains.Add(new Domain { Name = "telecompaper.com", ReadableName = "Telecompaper", Rank = ++i });
            _domains.Add(new Domain { Name = "streetinsider.com", ReadableName = "Street Insider", Rank = ++i });
            _domains.Add(new Domain { Name = "pressreader.com", ReadableName = "Press Reader", Rank = ++i });
            _domains.Add(new Domain { Name = "chinimandi.com", ReadableName = "Chini Mandi", Rank = ++i });
            _domains.Add(new Domain { Name = "manoramayearbook.in", ReadableName = "Manorama Yearbook", Rank = ++i });
            _domains.Add(new Domain { Name = "indiashippingnews.com", ReadableName = "India Shipping News", Rank = ++i });
            _domains.Add(new Domain { Name = "powerline.net.in", ReadableName = "Power Line", Rank = ++i });
            _domains.Add(new Domain { Name = "dailypioneer.com", ReadableName = "Daily Pioneer", Rank = ++i });
            _domains.Add(new Domain { Name = "globalgovernancenews.com", ReadableName = "Global Governance News", Rank = ++i });
            _domains.Add(new Domain { Name = "crosstownnews.in", ReadableName = "Crosstown News", Rank = ++i });
            _domains.Add(new Domain { Name = "blog.mygov.in", ReadableName = "MyGov Blog", Rank = ++i });
            _domains.Add(new Domain { Name = "aviationworld.in", ReadableName = "Aviation World", Rank = ++i });
            _domains.Add(new Domain { Name = "orissadiary.com", ReadableName = "Orissa Diary", Rank = ++i });
            _domains.Add(new Domain { Name = "newsage.in", ReadableName = "News Age", Rank = ++i });
            _domains.Add(new Domain { Name = "bureaucratsindia.in", ReadableName = "Bureaucrats India", Rank = ++i });
            _domains.Add(new Domain { Name = "newsarenaindia.com", ReadableName = "News Arena India", Rank = ++i });
            _domains.Add(new Domain { Name = "therahnuma.com", ReadableName = "The Rahnuma", Rank = ++i });
            _domains.Add(new Domain { Name = "newsdrum.in", ReadableName = "News Drum", Rank = ++i });
            _domains.Add(new Domain { Name = "travelbizmonitor.com", ReadableName = "Travel Biz Monitor", Rank = ++i });
            _domains.Add(new Domain { Name = "thenewsminute.com", ReadableName = "The News Minute", Rank = ++i });
            _domains.Add(new Domain { Name = "timesnownews.com", ReadableName = "Times Now News", Rank = ++i });
            _domains.Add(new Domain { Name = "ianslive.in", ReadableName = "IANS Live", Rank = ++i });
            _domains.Add(new Domain { Name = "english.varthabharati.in", ReadableName = "Vartha Bharati", Rank = ++i });
            _domains.Add(new Domain { Name = "guwahatiplus.com", ReadableName = "Guwahati Plus", Rank = ++i });
            _domains.Add(new Domain { Name = "ptcnews.tv", ReadableName = "PTC News", Rank = ++i });
            _domains.Add(new Domain { Name = "kashmirnewsobserver.com", ReadableName = "Kashmir News Observer", Rank = ++i });
            _domains.Add(new Domain { Name = "bhaskarenglish.in", ReadableName = "Bhaskar English", Rank = ++i });
            _domains.Add(new Domain { Name = "htsyndication.com", ReadableName = "HT Syndication", Rank = ++i });
            _domains.Add(new Domain { Name = "hubnetwork.in", ReadableName = "Hub Network", Rank = ++i });
            _domains.Add(new Domain { Name = "varindia.com", ReadableName = "VAR India", Rank = ++i });
            _domains.Add(new Domain { Name = "drishtiias.com", ReadableName = "Drishti IAS", Rank = ++i });
            _domains.Add(new Domain { Name = "caravanmagazine.in", ReadableName = "Caravan Magazine", Rank = ++i });
            _domains.Add(new Domain { Name = "techobserver.in", ReadableName = "Tech Observer", Rank = ++i });
            _domains.Add(new Domain { Name = "entrackr.com", ReadableName = "Entrackr", Rank = ++i });
            _domains.Add(new Domain { Name = "newsonprojects.com", ReadableName = "News on Projects", Rank = ++i });
            _domains.Add(new Domain { Name = "entrepreneurindia.com", ReadableName = "Entrepreneur India", Rank = ++i });
            _domains.Add(new Domain { Name = "autocarpro.in", ReadableName = "Autocar Pro", Rank = ++i });
            _domains.Add(new Domain { Name = "businessnewsweek.in", ReadableName = "Business News Week", Rank = ++i });
            _domains.Add(new Domain { Name = "ibef.org", ReadableName = "IBEF", Rank = ++i });
            _domains.Add(new Domain { Name = "jharkhandstatenews.com", ReadableName = "Jharkhand State News", Rank = ++i });
            _domains.Add(new Domain { Name = "biginfo.in", ReadableName = "Big Info", Rank = ++i });
            _domains.Add(new Domain { Name = "policyedge.in", ReadableName = "Policy Edge", Rank = ++i });
            _domains.Add(new Domain { Name = "odishatv.in", ReadableName = "Odisha TV", Rank = ++i });
            _domains.Add(new Domain { Name = "argusenglish.in", ReadableName = "Argus English", Rank = ++i });
            _domains.Add(new Domain { Name = "siliconindia.com", ReadableName = "Silicon India", Rank = ++i });
            _domains.Add(new Domain { Name = "chemanalyst.com", ReadableName = "Chem Analyst", Rank = ++i });
            _domains.Add(new Domain { Name = "thequint.com", ReadableName = "The Quint", Rank = ++i });
            _domains.Add(new Domain { Name = "itvoice.in", ReadableName = "IT Voice", Rank = ++i });
            _domains.Add(new Domain { Name = "expresscomputer.in", ReadableName = "Express Computer", Rank = ++i });
            _domains.Add(new Domain { Name = "kalingatv.com", ReadableName = "Kalinga TV", Rank = ++i });
            _domains.Add(new Domain { Name = "thesentimes.com", ReadableName = "The Sentimes", Rank = ++i });
            _domains.Add(new Domain { Name = "mea.gov.in", ReadableName = "MEA", Rank = ++i });
            _domains.Add(new Domain { Name = "businessnewsthisweek.com", ReadableName = "Business News This Week", Rank = ++i });
            _domains.Add(new Domain { Name = "indiatodayne.in", ReadableName = "India Today NE", Rank = ++i });
            _domains.Add(new Domain { Name = "in.ign.com", ReadableName = "IGN India", Rank = ++i });
            _domains.Add(new Domain { Name = "crictoday.com", ReadableName = "Cric Today", Rank = ++i });
            _domains.Add(new Domain { Name = "the420.in", ReadableName = "The 420", Rank = ++i });
            _domains.Add(new Domain { Name = "jagranjosh.com", ReadableName = "Jagran Josh", Rank = ++i });
            _domains.Add(new Domain { Name = "indiaipo.in", ReadableName = "India IPO", Rank = ++i });
            _domains.Add(new Domain { Name = "mid - day.com", ReadableName = "Mid Day", Rank = ++i });
            _domains.Add(new Domain { Name = "etnownews.com", ReadableName = "ET Now News", Rank = ++i });
            _domains.Add(new Domain { Name = "psuwatch.com", ReadableName = "PSU Watch", Rank = ++i });
            _domains.Add(new Domain { Name = "thebridgechronicle.com", ReadableName = "The Bridge Chronicle", Rank = ++i });
            _domains.Add(new Domain { Name = "inkl.com", ReadableName = "INKL", Rank = ++i });
            _domains.Add(new Domain { Name = "indiatechnologynews.in", ReadableName = "India Technology News", Rank = ++i });
            _domains.Add(new Domain { Name = "univest.in", ReadableName = "Univest", Rank = ++i });
            _domains.Add(new Domain { Name = "fibre2fashion.com", ReadableName = "Fibre2Fashion", Rank = ++i });
            _domains.Add(new Domain { Name = "tele.net.in", ReadableName = "Tele.net", Rank = ++i });
            _domains.Add(new Domain { Name = "onlykashmir.in", ReadableName = "Only Kashmir", Rank = ++i });
            _domains.Add(new Domain { Name = "earthnews.in", ReadableName = "Earth News", Rank = ++i });
            _domains.Add(new Domain { Name = "cseindia.org", ReadableName = "CSE India", Rank = ++i });
            _domains.Add(new Domain { Name = "polymerupdate.com", ReadableName = "Polymer Update", Rank = ++i });
            _domains.Add(new Domain { Name = "verdictum.in", ReadableName = "Verdictum", Rank = ++i });
            _domains.Add(new Domain { Name = "homegrown.co.in", ReadableName = "Homegrown", Rank = ++i });
            _domains.Add(new Domain { Name = "indiasworld.in", ReadableName = "India's World", Rank = ++i });
            _domains.Add(new Domain { Name = "idsa.in", ReadableName = "IDSA", Rank = ++i });
            _domains.Add(new Domain { Name = "newsgram.com", ReadableName = "News Gram", Rank = ++i });
            _domains.Add(new Domain { Name = "medicaldialogues.in", ReadableName = "Medical Dialogues", Rank = ++i });
            _domains.Add(new Domain { Name = "bangaloremirror.indiatimes.com", ReadableName = "Bangalore Mirror", Rank = ++i });

            _domains.Add(new Domain { Name = "bwpeople.in", ReadableName = "BW People", Rank = ++i });
            _domains.Add(new Domain { Name = "northeasttoday.in", ReadableName = "North East Today", Rank = ++i });
            _domains.Add(new Domain { Name = "scconline.com", ReadableName = "SCC Online", Rank = ++i });
            _domains.Add(new Domain { Name = "fnbnews.com", ReadableName = "FNB News", Rank = ++i });
            _domains.Add(new Domain { Name = "news.webindia123.com", ReadableName = "Web India 123", Rank = ++i });
            _domains.Add(new Domain { Name = "digitalterminal.in", ReadableName = "Digital Terminal", Rank = ++i });
            _domains.Add(new Domain { Name = "trak.in", ReadableName = "Trak.in", Rank = ++i });
            _domains.Add(new Domain { Name = "thepackman.in", ReadableName = "The Packman", Rank = ++i });
            _domains.Add(new Domain { Name = "thepamphlet.in", ReadableName = "The Pamphlet", Rank = ++i });
            _domains.Add(new Domain { Name = "udayavani.com", ReadableName = "Udayavani", Rank = ++i });
            _domains.Add(new Domain { Name = "iadb.in", ReadableName = "IADB", Rank = ++i });
            _domains.Add(new Domain { Name = "goal.com", ReadableName = "Goal", Rank = ++i });
        }

        /// <summary>
        /// Extracts the domain name from the Href property.
        /// Examples: "https://timesofindia.com/..." → "Times of India"
        /// </summary>
        public (string, string) ExtractDomain(string href)
        {
            ArgumentNullException.ThrowIfNull(href);
            if (string.IsNullOrEmpty(href))
            {
                return ("Unknown", string.Empty);
            }

            try
            {
                var uri = new Uri(href);
                var hostParts = uri.Host.Split('.');

                var readableDomain = _domains.FirstOrDefault(d => href.IndexOf(d.Name, StringComparison.OrdinalIgnoreCase) > -1);

                if (readableDomain != null)
                {
                    return (readableDomain.ReadableName, string.Empty);
                }
                if (href.IndexOf(".com", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var domainName = href.Substring(0, href.IndexOf(".com", StringComparison.OrdinalIgnoreCase))
                        .Replace("https://www.", "")
                        .Replace("http://www.", "")
                        .Replace("www.", "") 
                        .Replace("https://", "")
                        .Replace("http://", "")
                        + ".com";
                    return ("Unknown- " + domainName, domainName);
                }
                else if (href.IndexOf(".in", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var domainName = href.Substring(0, href.IndexOf(".in", StringComparison.OrdinalIgnoreCase))
                        .Replace("https://www.", "")
                        .Replace("http://www.", "")
                        .Replace("www.", "")
                        .Replace("https://", "")
                        .Replace("http://", "")
                        + ".in";
                    return ("Unknown- " + domainName, domainName);
                }
                else if (href.IndexOf(".org", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var domainName = href.Substring(0, href.IndexOf(".org", StringComparison.OrdinalIgnoreCase))
                        .Replace("https://www.", "")
                        .Replace("http://www.", "")
                        .Replace("www.", "")
                        .Replace("https://", "")
                        .Replace("http://", "")
                        + ".org";
                    return ("Unknown- " + domainName, domainName);
                }
                else if (href.IndexOf(".net", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    var domainName = href.Substring(0, href.IndexOf(".net", StringComparison.OrdinalIgnoreCase))
                        .Replace("https://www.", "")
                        .Replace("http://www.", "")
                        .Replace("www.", "")
                        .Replace("https://", "")
                        .Replace("http://", "")
                        + ".net";
                    return ("Unknown- " + domainName, domainName);
                }

                // Get the domain name (second-to-last part before TLD)
                if (hostParts.Length >= 2)
                {
                    string domainName = hostParts[^2]; // Get second-to-last part

                    return ("Unknown" + System.Text.RegularExpressions.Regex.Replace(
                            domainName,
                            "(?<!^)(?=[A-Z])",
                            " "
                        ), string.Empty);
                }
                else
                {
                    return ("Unknown" + uri.Host, string.Empty);
                }
            }
            catch
            {
                return ("Unknown", string.Empty);
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
