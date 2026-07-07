using InvestmentResearch.Model;
using Microsoft.Extensions.Options;
using Mscc.GenerativeAI;
using Mscc.GenerativeAI.Types;

namespace InvestmentResearch.Service
{
    public class GeminiService : IGeminiService
    {
        private readonly List<GenerativeModel> _models = new List<GenerativeModel>();
        private int _generativeModelIndex = 0;
        private int _totalGenerativeModelIndex = 0;
        private readonly AppSettings _appSettings;

        public GeminiService(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value ?? throw new ArgumentNullException(nameof(appSettings));
        }

        public GenerativeModel[] Models
        {
            get
            {
                if(this._models == null || _models == new List<GenerativeModel>() || _models.Count == 0)
                {
                    var googleAI = new GoogleAI(apiKey: _appSettings.GeminiApiKey1);
                   
                    var model = googleAI.GenerativeModel(model: Mscc.GenerativeAI.Types.Model.Gemini25Flash);
                    if (_models == null)
                    {
                        throw new ArgumentNullException(nameof(_models));
                    }
                    _models.Add(model);

                    if (!string.IsNullOrEmpty(_appSettings.GeminiApiKey2))
                    {
                        googleAI = new GoogleAI(apiKey: _appSettings.GeminiApiKey2);
                        model = googleAI.GenerativeModel(model: Mscc.GenerativeAI.Types.Model.Gemini25Flash);
                        _models.Add(model);
                    }

                    if (!string.IsNullOrEmpty(_appSettings.GeminiApiKey3))
                    {
                        googleAI = new GoogleAI(apiKey: _appSettings.GeminiApiKey3);
                        model = googleAI.GenerativeModel(model: Mscc.GenerativeAI.Types.Model.Gemini25Flash);
                        _models.Add(model);
                    }

                    if (!string.IsNullOrEmpty(_appSettings.GeminiApiKey4))
                    {
                        googleAI = new GoogleAI(apiKey: _appSettings.GeminiApiKey4);
                        model = googleAI.GenerativeModel(model: Mscc.GenerativeAI.Types.Model.Gemini25Flash);
                        _models.Add(model);
                    }

                    if (!string.IsNullOrEmpty(_appSettings.GeminiApiKey5))
                    {
                        googleAI = new GoogleAI(apiKey: _appSettings.GeminiApiKey5);
                        model = googleAI.GenerativeModel(model: Mscc.GenerativeAI.Types.Model.Gemini25Flash);
                        _models.Add(model);
                    }
                }

                this._totalGenerativeModelIndex = _models.Count - 1;
                GenerativeModel[] generativeModels = new GenerativeModel[_models.Count];
                foreach (var model in _models) {
                    generativeModels[_models.IndexOf(model)] = model;
                }

                return _models.ToArray<GenerativeModel>();
            }
        }

        public async Task<string> RSSFeedAsync(string systemInstruction, string userData)
        {
            // 1. Configure the request container natively supporting strict JSON mode
            var request = new GenerateContentRequest
            {
                Contents = new List<Content>
                {
                    new Content($"System: {systemInstruction}\n\nUser Data:\n{userData}")
                },
                GenerationConfig = new GenerationConfig
                {
                    ResponseMimeType = "application/json", // Forces a clean, parser-ready JSON string
                    Temperature = 0.0
                }
            };

            // 2. Fire request safely using your active model index
            var response = await this.Models[this._generativeModelIndex].GenerateContent(request);

            if (response == null || string.IsNullOrEmpty(response.Text))
            {
                return string.Empty;
            }

            // Since ResponseMimeType is application/json, response.Text is immediately a valid JSON string
            return response.Text.Trim();
        }

        public async Task<CompanyResult> GenerateContent(string prompt)
        {
            // Implement content generation logic using the Gemini model
            CompanyResult? companyResult = new();
            int maxRetries = 5;
            int retryCount = 0;

            while (retryCount < maxRetries)
            {
                try
                {
                    var request = new GenerateContentRequest
                    {
                        Contents = new List<Content> { new Content(prompt) },
                        Tools = new Tools
                            {
                                new Tool
                                {
                                    GoogleSearch = new GoogleSearch()
                                }
                            }
                    };
                    var response = await this.Models[this._generativeModelIndex].GenerateContent(request);
                    companyResult = new CompanyResult
                    {
                        Response = response.Text == null ? "" : response.Text.Trim().ToLower().Contains("no news found today") ? "" : response.Text
                    };

                    Console.WriteLine($"    ✓ Completed");
                    break; // Success, exit retry loop
                }
                catch (Exception ex)
                {
                    retryCount++;
                    Console.WriteLine($"    ✗ Error (Attempt {retryCount}/{maxRetries}): {ex.Message}");

                    if (retryCount < maxRetries)
                    {
                        // Switch to next model/API key for retry
                        if (this._generativeModelIndex >= this._totalGenerativeModelIndex)
                        {
                            this._generativeModelIndex = 0;
                        }
                        else
                        {
                            this._generativeModelIndex++;
                        }

                        // Optional: Add delay before retrying
                        await Task.Delay(1000);
                    }
                    else
                    {
                        Console.WriteLine($"    ✗ Max retries ({maxRetries}) exceeded. Giving up.");
                    }
                }
            }

            return companyResult;
        }
    }
}
