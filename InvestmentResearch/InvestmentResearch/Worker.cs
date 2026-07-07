using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Hosting;
using InvestmentResearch.Service;

namespace InvestmentResearch
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IPipelineService _pipeline;

        public Worker(ILogger<Worker> logger, IPipelineService pipeline)
        {
            _logger = logger;
            _pipeline = pipeline;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Worker started.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    _logger.LogInformation("Running pipeline...");

                    _pipeline.Run();

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
