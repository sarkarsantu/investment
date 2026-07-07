namespace InvestmentResearch.Model
{
    public class Prompt
    {
        public string FileName { get; set; } = string.Empty;
        public bool IncludeCompanies { get; set; } = false;
        public bool DailyCall { get; set; } = false;
        public bool WeeklyCall { get; set; } = false;
        public bool MonthlyCall { get; set; } = false;
        public bool CompanyWise { get; set; } = false;
        public bool SectorWise { get; set; } = false;
    }
}
