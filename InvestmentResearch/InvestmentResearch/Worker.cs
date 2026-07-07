using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using InvestmentResearch.Service;

namespace InvestmentResearch
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPipelineService _pipeline;
        private readonly IGitHubHelper _gitHubHelper;

        public Worker(ILogger<Worker> logger, IPipelineService pipeline, IGitHubHelper gitHubHelper)
        {
            _logger = logger;
            _pipeline = pipeline;
            _gitHubHelper = gitHubHelper;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");

            // Test GitHub token on startup
            _logger.LogInformation("Testing GitHub token permissions...");
            bool tokenValid = await _gitHubHelper.TestGitHubTokenAsync();
            if (!tokenValid)
            {
                _logger.LogError("GitHub token test failed! Please check your token configuration.");
                return;
            }

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Running pipeline...");

                    _pipeline.RunAsync();

                    _logger.LogInformation("Pipeline completed.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Pipeline failed.");
                }

                // ⏱ Run every 1 hour (you can change)
                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}
