using Application.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ForgottenEmpire.HostedServices
{
    public class NotionSyncHostedService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<NotionSyncHostedService> _logger;
        private readonly IConfiguration _configuration;

        public NotionSyncHostedService(
            IServiceScopeFactory scopeFactory,
            ILogger<NotionSyncHostedService> logger,
            IConfiguration configuration)
        {
            _scopeFactory = scopeFactory ?? throw new ArgumentNullException(nameof(scopeFactory));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var pollingSeconds = _configuration.GetValue<int>("Notion:PollingIntervalSeconds", 300);
            _logger.LogInformation("Notion sync worker started. Polling every {PollingSeconds} seconds.", pollingSeconds);

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope = _scopeFactory.CreateScope();
                    var syncService = scope.ServiceProvider.GetRequiredService<INotionSyncService>();
                    await syncService.SyncFromNotionAsync(stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error during Notion sync execution.");
                }

                try
                {
                    await Task.Delay(TimeSpan.FromSeconds(pollingSeconds), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }

            _logger.LogInformation("Notion sync worker is stopping.");
        }
    }
}
