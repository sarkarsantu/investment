namespace InvestmentResearch.Model;

public class Link
{
    public int SlNo { get; set; } = 0;
    public int Rank { get; set; } = 0;
    public string Id { get; set; }
    public string Title { get; set; }
    public string Href { get; set; }
    public string Published { get; set; } = DateTime.Today.ToString("yyyy-MM-dd");
    public string Domain { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsRelevant { get; set; } = true;
    public bool IsDuplicate { get; set; } = false;
    public string? Theme { get; set; } = string.Empty;
}
