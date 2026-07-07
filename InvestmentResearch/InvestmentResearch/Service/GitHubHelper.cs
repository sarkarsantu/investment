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

            // Ensure the directory exists in GitHub by creating a .gitkeep file if needed
            string directory = Path.GetDirectoryName(gitHubPath);
            if (!string.IsNullOrEmpty(directory) && directory != ".")
            {
                bool dirCreated = await EnsureGitHubDirectoryExists(directory);
                if (!dirCreated)
                {
                    Console.WriteLine($"??  Warning: Could not create directory structure, but will attempt file upload anyway");
                }
            }

            string fileContent = File.ReadAllText(filePath);
            string encodedContent = Convert.ToBase64String(Encoding.UTF8.GetBytes(fileContent));

            string url = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{gitHubPath}";

            Console.WriteLine($"\n??  Attempting to upload to: {url}");

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
                    Console.WriteLine($"??  File exists. SHA: {sha}");
                }
                else if (getResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    Console.WriteLine($"??  File does not exist yet. Creating new file.");
                }
                else
                {
                    Console.WriteLine($"??  Unexpected response when checking file: {getResponse.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"??  File does not exist yet (Exception: {ex.Message}). Creating new file.");
            }

            // Create payload - only include sha if it exists
            var payloadDict = new Dictionary<string, object>
            {
                { "message", $"Add investment research report: {Path.GetFileName(filePath)}" },
                { "content", encodedContent }
            };

            if (!string.IsNullOrEmpty(sha))
            {
                payloadDict["sha"] = sha;
            }

            string jsonPayload = JsonSerializer.Serialize(payloadDict);
            var content_put = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

            Console.WriteLine($"??  Sending PUT request to GitHub...");
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
                Console.WriteLine($"   Status: {response.StatusCode} ({(int)response.StatusCode})");
                Console.WriteLine($"   URL: {url}");

                // Parse error details
                try
                {
                    using (JsonDocument doc = JsonDocument.Parse(errorContent))
                    {
                        if (doc.RootElement.TryGetProperty("message", out var msg))
                        {
                            Console.WriteLine($"   Error: {msg.GetString()}");
                        }
                    }
                }
                catch { }

                Console.WriteLine($"   Full Response: {errorContent}");
                return false;
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Error uploading file: {ex.Message}");
            Console.WriteLine($"   Stack Trace: {ex.StackTrace}");
            return false;
        }
    }

    /// <summary>
    /// Ensures that a directory exists in GitHub by creating a .gitkeep file
    /// </summary>
    private async Task<bool> EnsureGitHubDirectoryExists(string gitHubDirectory)
    {
        try
        {
            Console.WriteLine($"??  Checking if directory exists: {gitHubDirectory}");

            // Try to get the directory (GitHub returns files in a directory)
            string checkUrl = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{gitHubDirectory}";
            var checkResponse = await _httpClient.GetAsync(checkUrl);

            if (checkResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"? Directory already exists: {gitHubDirectory}");
                return true;
            }

            // Directory doesn't exist, create it with a .gitkeep file
            if (checkResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                Console.WriteLine($"??  Directory does not exist. Creating: {gitHubDirectory}");

                string gitkeepPath = $"{gitHubDirectory}/.gitkeep";
                string gitkeepUrl = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{gitkeepPath}";

                var payloadDict = new Dictionary<string, object>
                {
                    { "message", $"Create {gitHubDirectory} directory" },
                    { "content", Convert.ToBase64String(Encoding.UTF8.GetBytes("")) }
                };

                string jsonPayload = JsonSerializer.Serialize(payloadDict);
                var content_put = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

                var createResponse = await _httpClient.PutAsync(gitkeepUrl, content_put);

                if (createResponse.IsSuccessStatusCode)
                {
                    Console.WriteLine($"? Directory created successfully: {gitHubDirectory}");
                    return true;
                }
                else
                {
                    var errorContent = await createResponse.Content.ReadAsStringAsync();
                    Console.WriteLine($"? Failed to create directory: {gitHubDirectory}");
                    Console.WriteLine($"   Status: {createResponse.StatusCode}");
                    Console.WriteLine($"   Error: {errorContent}");
                    return false;
                }
            }

            Console.WriteLine($"??  Unexpected response when checking directory: {checkResponse.StatusCode}");
            return false;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Error ensuring directory exists: {ex.Message}");
            return false;
        }
    }

    /// <summary>
    /// Diagnostic method to test GitHub token validity and permissions
    /// </summary>
    public async Task<bool> TestGitHubTokenAsync()
    {
        try
        {
            Console.WriteLine("\n=== Testing GitHub Token ===\n");

            // Test 1: Check token validity
            Console.WriteLine("1??  Testing token validity...");
            var userResponse = await _httpClient.GetAsync("https://api.github.com/user");
            if (!userResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"? Token is invalid or expired. Status: {userResponse.StatusCode}");
                return false;
            }

            var userData = await userResponse.Content.ReadAsStringAsync();
            using (JsonDocument doc = JsonDocument.Parse(userData))
            {
                var login = doc.RootElement.GetProperty("login").GetString();
                Console.WriteLine($"? Token is valid. Authenticated as: {login}\n");
            }

            // Test 2: Check repository access
            Console.WriteLine("2??  Testing repository access...");
            var repoUrl = $"https://api.github.com/repos/{_owner}/{_repo}";
            var repoResponse = await _httpClient.GetAsync(repoUrl);
            if (!repoResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"? No access to repository. Status: {repoResponse.StatusCode}");
                var errorContent = await repoResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"   Error: {errorContent}\n");
                return false;
            }
            Console.WriteLine($"? Repository access confirmed: {_owner}/{_repo}\n");

            // Test 3: Check scopes in response headers
            Console.WriteLine("3??  Checking token scopes...");
            if (userResponse.Headers.TryGetValues("X-OAuth-Scopes", out var scopes))
            {
                var scopeList = string.Join(", ", scopes);
                Console.WriteLine($"? Token Scopes: {scopeList}\n");
            }

            // Test 4: Try to list directory contents
            Console.WriteLine("4??  Testing directory read access...");
            var dirUrl = $"https://api.github.com/repos/{_owner}/{_repo}/contents/investment-reports";
            var dirResponse = await _httpClient.GetAsync(dirUrl);
            if (dirResponse.IsSuccessStatusCode)
            {
                var dirContent = await dirResponse.Content.ReadAsStringAsync();
                using (JsonDocument doc = JsonDocument.Parse(dirContent))
                {
                    if (doc.RootElement.ValueKind == System.Text.Json.JsonValueKind.Array)
                    {
                        Console.WriteLine($"? Directory exists and is readable. Files: {doc.RootElement.GetArrayLength()}\n");
                    }
                }
            }
            else
            {
                Console.WriteLine($"??  Directory read test failed: {dirResponse.StatusCode}\n");
            }

            // Test 5: Try a test write operation
            Console.WriteLine("5??  Testing write permissions with a test file...");
            string testPath = $"investment-reports/test-{DateTime.Now:yyyyMMdd-HHmmss}.txt";
            string testUrl = $"https://api.github.com/repos/{_owner}/{_repo}/contents/{testPath}";

            var testPayloadDict = new Dictionary<string, object>
            {
                { "message", "Test write permission" },
                { "content", Convert.ToBase64String(Encoding.UTF8.GetBytes("test")) }
            };

            string testJsonPayload = JsonSerializer.Serialize(testPayloadDict);
            var testContent = new StringContent(testJsonPayload, Encoding.UTF8, "application/json");
            var testWriteResponse = await _httpClient.PutAsync(testUrl, testContent);

            if (testWriteResponse.IsSuccessStatusCode)
            {
                Console.WriteLine($"? Write permission confirmed! Test file created.\n");

                // Clean up test file
                var testGetResponse = await _httpClient.GetAsync(testUrl);
                if (testGetResponse.IsSuccessStatusCode)
                {
                    var testFileContent = await testGetResponse.Content.ReadAsStringAsync();
                    using (JsonDocument doc = JsonDocument.Parse(testFileContent))
                    {
                        if (doc.RootElement.TryGetProperty("sha", out var testSha))
                        {
                            var cleanupPayload = new Dictionary<string, object>
                            {
                                { "message", "Clean up test file" },
                                { "sha", testSha.GetString() }
                            };
                            string cleanupJson = JsonSerializer.Serialize(cleanupPayload);
                            var cleanupContent = new StringContent(cleanupJson, Encoding.UTF8, "application/json");
                            await _httpClient.DeleteAsync(testUrl);
                        }
                    }
                }
            }
            else
            {
                var testError = await testWriteResponse.Content.ReadAsStringAsync();
                Console.WriteLine($"? Write permission test failed: {testWriteResponse.StatusCode}");
                Console.WriteLine($"   Error: {testError}\n");
                return false;
            }

            Console.WriteLine("=== All Tests Passed! ===\n");
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"? Error testing token: {ex.Message}\n");
            return false;
        }
    }
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
