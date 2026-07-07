namespace InvestmentResearch.Service
{
    public interface IGitHubHelper
    {
        Task<bool> UploadFileAsync(string filePath, string gitHubPath);
        Task<bool> TestGitHubTokenAsync();

       // Task<bool> UploadMultipleFilesAsync(string localFolder, string githubFolder);
    }
}
