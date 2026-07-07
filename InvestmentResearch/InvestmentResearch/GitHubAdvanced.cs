namespace InvestmentResearch;

using InvestmentResearch.Model;
using InvestmentResearch.Service;
using Microsoft.Extensions.Options;

public class GitHubAdvanced
{
    private readonly GitHubHelper _gitHubHelper;
    private readonly AppSettings _appSettings;
    private readonly string _token;
    private readonly string _owner;
    private readonly string _repo;

    public GitHubAdvanced(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings.Value;
        _gitHubHelper = new GitHubHelper(appSettings ?? throw new Exception("appSettings file did not setup."));
        _appSettings = appSettings.Value;
        _token = _appSettings.GitHubToken;
        _owner = _appSettings.GitHubOwner;
        _repo = _appSettings.GitHubRepo;
    }

    //public async Task<bool> CreateFolderIfNotExistsAsync(string folderPath)
    //{
    //    try
    //    {
    //        // GitHub doesn't have folders, but we can create a .gitkeep file to simulate a folder
    //        string keepFilePath = $"{folderPath}/.gitkeep";
    //        var httpClient = new HttpClient();
    //        httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestmentResearch");

    //        string url = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{keepFilePath}";
    //        var getResponse = await httpClient.GetAsync(url);

    //        // If .gitkeep doesn't exist, no need to create it - files can be directly uploaded to folder
    //        return true;
    //    }
    //    catch
    //    {
    //        return true; // Folder operations are simulated in GitHub
    //    }
    //}

    //public async Task<List<string>> ListFilesInFolderAsync(string folderPath)
    //{
    //    var files = new List<string>();
    //    try
    //    {
    //        var httpClient = new HttpClient();
    //        httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestmentResearch");

    //        string url = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{folderPath}";
    //        var response = await httpClient.GetAsync(url);

    //        if (response.IsSuccessStatusCode)
    //        {
    //            var content = await response.Content.ReadAsStringAsync();
    //            using (JsonDocument doc = JsonDocument.Parse(content))
    //            {
    //                if (doc.RootElement.ValueKind == JsonValueKind.Array)
    //                {
    //                    foreach (var element in doc.RootElement.EnumerateArray())
    //                    {
    //                        if (element.GetProperty("type").GetString() == "file")
    //                        {
    //                            var name = element.GetProperty("name").GetString();
    //                            if (name != null)
    //                            {
    //                                files.Add(name);
    //                            }
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error listing files: {ex.Message}");
    //    }

    //    return files;
    //}

    //public async Task<bool> DeleteFileAsync(string filePath)
    //{
    //    try
    //    {
    //        var httpClient = new HttpClient();
    //        httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestmentResearch");

    //        string url = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{filePath}";

    //        // First, get the file SHA
    //        var getResponse = await httpClient.GetAsync(url);
    //        if (!getResponse.IsSuccessStatusCode)
    //        {
    //            Console.WriteLine($"File not found: {filePath}");
    //            return false;
    //        }

    //        var content = await getResponse.Content.ReadAsStringAsync();
    //        string? sha = null;
    //        using (JsonDocument doc = JsonDocument.Parse(content))
    //        {
    //            sha = doc.RootElement.GetProperty("sha").GetString();
    //        }

    //        if (string.IsNullOrEmpty(sha))
    //        {
    //            Console.WriteLine("Could not retrieve file SHA");
    //            return false;
    //        }

    //        // Delete the file
    //        var payload = new { message = $"Delete {Path.GetFileName(filePath)}", sha = sha };
    //        string jsonPayload = JsonSerializer.Serialize(payload);
    //        var deleteContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

    //        var deleteResponse = await httpClient.DeleteAsync(url);
    //        if (deleteResponse.IsSuccessStatusCode)
    //        {
    //            Console.WriteLine($"? Successfully deleted: {filePath}");
    //            return true;
    //        }
    //        else
    //        {
    //            Console.WriteLine($"? Failed to delete: {filePath}");
    //            return false;
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"Error deleting file: {ex.Message}");
    //        return false;
    //    }
    //}
}
