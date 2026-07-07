namespace InvestmentResearch.Helper;

public class HttpClientHelper
{
    // Track last request time per domain to implement rate limiting
    private static readonly Dictionary<string, DateTime> _lastRequestTime = new();
    private static readonly object _lock = new object();

    // Minimum delay between requests to the same domain (in milliseconds)
    private const int MinDelayBetweenRequests = 2000; // 2 seconds

    public static HttpClient CreateBrowserLikeClient()
    {
        var handler = new HttpClientHandler
        {
            UseCookies = true,
            CookieContainer = new System.Net.CookieContainer(),
            AllowAutoRedirect = true,
            // Enable automatic decompression of gzip/deflate
            AutomaticDecompression = System.Net.DecompressionMethods.GZip |
                                      System.Net.DecompressionMethods.Deflate
        };

        var client = new HttpClient(handler, disposeHandler: true);
        // Increased from 15s to 30s to handle slow sites
        client.Timeout = TimeSpan.FromSeconds(30);

        // Set browser-like headers to avoid 403 Forbidden
        client.DefaultRequestHeaders.Clear();
        client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8");
        client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.9");
        // Only request gzip/deflate compression that we can handle - exclude brotli (br)
        client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate");
        client.DefaultRequestHeaders.Add("Cache-Control", "max-age=0");
        client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Mode", "navigate");
        client.DefaultRequestHeaders.Add("Sec-Fetch-Site", "none");

        return client;
    }

    /// <summary>
    /// Enforces rate limiting per domain to avoid 429 Too Many Requests errors
    /// </summary>
    public static void EnforceRateLimit(string url)
    {
        try
        {
            var uri = new Uri(url);
            string domain = uri.Host;

            lock (_lock)
            {
                if (_lastRequestTime.TryGetValue(domain, out var lastTime))
                {
                    var elapsed = DateTime.UtcNow - lastTime;
                    if (elapsed.TotalMilliseconds < MinDelayBetweenRequests)
                    {
                        var delayMs = (int)(MinDelayBetweenRequests - elapsed.TotalMilliseconds);
                        System.Threading.Thread.Sleep(delayMs);
                    }
                }

                _lastRequestTime[domain] = DateTime.UtcNow;
            }
        }
        catch
        {
            // If parsing fails, just continue without rate limiting
        }
    }

    /// <summary>
    /// Gets content from URL with retry logic and exponential backoff for rate limiting and server errors
    /// </summary>
    public static async Task<string> GetStringWithRetryAsync(string url, int maxRetries = 5)
    {
        using var client = CreateBrowserLikeClient();

        for (int attempt = 0; attempt < maxRetries; attempt++)
        {
            try
            {
                EnforceRateLimit(url);
                var response = await client.GetAsync(url);

                // Handle rate limiting (429)
                if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                {
                    if (attempt < maxRetries - 1)
                    {
                        var delayMs = (int)Math.Pow(2, attempt) * 3000; // 3s, 6s, 12s, 24s, 48s
                        Console.WriteLine($"⏳ Rate limited (429) on {url}. Retrying in {delayMs}ms...");
                        await Task.Delay(delayMs);
                        continue;
                    }
                }

                // Handle temporary server errors (502, 503, 504)
                if ((int)response.StatusCode >= 500 && (int)response.StatusCode < 600)
                {
                    if (attempt < maxRetries - 1)
                    {
                        var delayMs = (int)Math.Pow(2, attempt) * 2000; // 2s, 4s, 8s, 16s, 32s
                        Console.WriteLine($"⏳ Server error ({(int)response.StatusCode}) on {url}. Retrying in {delayMs}ms...");
                        await Task.Delay(delayMs);
                        continue;
                    }
                }

                // For other successful responses, return content
                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }

                // For other non-2xx errors, throw
                response.EnsureSuccessStatusCode();
            }
            catch (TaskCanceledException ex) when (attempt < maxRetries - 1)
            {
                // Timeout - retry with exponential backoff
                var delayMs = (int)Math.Pow(2, attempt) * 2000; // 2s, 4s, 8s, 16s, 32s
                Console.WriteLine($"⏳ Request timeout on {url}. Retrying in {delayMs}ms...");
                await Task.Delay(delayMs);
                continue;
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.TooManyRequests && attempt < maxRetries - 1)
            {
                var delayMs = (int)Math.Pow(2, attempt) * 3000;
                Console.WriteLine($"⏳ Rate limited (429) on {url}. Retrying in {delayMs}ms...");
                await Task.Delay(delayMs);
                continue;
            }
            catch (HttpRequestException ex) when (ex.StatusCode >= System.Net.HttpStatusCode.InternalServerError && attempt < maxRetries - 1)
            {
                var delayMs = (int)Math.Pow(2, attempt) * 2000;
                Console.WriteLine($"⏳ Server error on {url}. Retrying in {delayMs}ms...");
                await Task.Delay(delayMs);
                continue;
            }
        }

        throw new HttpRequestException($"Failed to retrieve {url} after {maxRetries} attempts");
    }
}
