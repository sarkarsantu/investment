namespace InvestmentResearch.Repository
{
    public interface IFileHelper
    {
        Task UploadtoGitHub(string fileName);

        Task FileSave(string fileName, string fileContent);

        Task<string> GetPrompt(string fileName, string sectorName = null!, string companyName = null!);
    }
}
