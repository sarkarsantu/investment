namespace InvestmentResearch.Model
{
    public class Company
    {
        private string _prompt = string.Empty;
        public long Id { get; set; }
        public string SectorName { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Prompt
        {
            get
            {
                return _prompt;
            }
            set
            {
                string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
                string yesterdayDate = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd");
                _prompt = value.Replace("\r\n", "\n").Replace("\r", "\n").Replace("{todayDate}", todayDate).Replace("{yesterdayDate}", yesterdayDate);
            }
        }
    }
}
