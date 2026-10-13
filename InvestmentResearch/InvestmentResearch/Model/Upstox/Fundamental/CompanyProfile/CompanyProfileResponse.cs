namespace InvestmentResearch.Model.Upstox.Fundamental.CompanyProfile;

public class CompanyProfileResponse
{
    public string Status { get; set; } = string.Empty;
    public Data Data { get; set; } = new Data();
}
