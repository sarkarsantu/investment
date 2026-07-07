namespace InvestmentResearch.Service;

using InvestmentResearch.Model;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

public class GitHubHelper : IGitHubHelper
{
    private readonly AppSettings _appSettings;
    private readonly string _gitHubToken;
    private readonly string _owner;
    private readonly string _repo;
    private readonly HttpClient _httpClient;
    private const string _outputFolder = "Output";

    public GitHubHelper(IOptions<AppSettings> appSettings)
    {
        _appSettings = appSettings?.Value ?? throw new ArgumentNullException(nameof(appSettings));
        _gitHubToken = _appSettings.GitHubToken;
        _owner = _appSettings.GitHubOwner;
        _repo = _appSettings.GitHubRepo;

        _httpClient = new HttpClient();
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _gitHubToken);
        _httpClient.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json");
        _httpClient.DefaultRequestHeaders.Add("User-Agent", "InvestmentResearch");
    }

    public async Task<bool> UploadFileAsync(string filePath, string gitHubPath)
    {
        try
        {
            if (!File.Exists(filePath))
            {
                Console.WriteLine($"File not found: {filePath}");
                return false;
            }

            string fileContent = File.ReadAllText(filePath);
            string encodedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

            string url = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{gitHubPath}";

            // Check if file already exists to get its SHA
            string? sha = null;
            try
            {
                var getResponse = await _httpClient.GetAsync(url);
                if (getResponse.IsSuccessStatusCode)
                {
                    var content = await getResponse.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(content))
                    {
                        sha = doc.RootElement.GetProperty("sha").GetString();
                    }
                }
            }
            catch
            {
                // File doesn't exist yet, that's fine
            }

            var payload = new
            {
                message = $"Add investment research report: {Path.GetFileName(filePath)}",
                content = encodedContent,
                sha
            };

            string jsonPayload = JsonSerializer.Serialize(payload);
            var content_put = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync(url, content_put);

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine($"? Successfully uploaded: {gitHubPath}");
                return true;
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"? Failed to upload: {gitHubPath}");
                Console.WriteLine($"  Status: {response.StatusCode}");
                Console.WriteLine($"  Error: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Error uploading file: {ex.Message}");
            return false;
        }
    }

    //public async Task<bool> UploadMultipleFilesAsync(string localFolder, string githubFolder)
    //{
    //    try
    //    {
    //        if (!Directory.Exists(localFolder))
    //        {
    //            Console.WriteLine($"Directory not found: {localFolder}");
    //            return false;
    //        }

    //        var htmlFiles = Directory.GetFiles(localFolder, "*.html");

    //        if (htmlFiles.Length == 0)
    //        {
    //            Console.WriteLine($"No HTML files found in: {localFolder}");
    //            return false;
    //        }

    //        Console.WriteLine($"\nUploading {htmlFiles.Length} file(s) to GitHub...");

    //        bool allSuccess = true;
    //        foreach (var filePath in htmlFiles)
    //        {
    //            string fileName = Path.GetFileName(filePath);
    //            string gitHubPath = $"{githubFolder}/{fileName}";

    //            bool success = await UploadFileAsync(filePath, gitHubPath);
    //            if (!success)
    //            {
    //                allSuccess = false;
    //            }
    //        }

    //        return allSuccess;
    //    }
    //    catch (Exception ex)
    //    {
    //        Console.WriteLine($"? Error uploading multiple files: {ex.Message}");
    //        return false;
    //    }
    //}
}
