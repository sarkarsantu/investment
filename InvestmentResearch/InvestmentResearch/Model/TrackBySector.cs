namespace InvestmentResearch.Model
{
    public class TrackBySector
    {
        public required string Sector { get; set; }
        public required int LinkCount { get; set; }
        public required int FinalLinkCount { get; set; }
        public required List<TrackByGoogleAlert> GoogleAlerts { get; set; }
    }
}
