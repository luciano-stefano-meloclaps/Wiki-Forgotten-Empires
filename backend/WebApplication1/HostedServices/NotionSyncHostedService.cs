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
            var notionSecret = _configuration["Notion:Secret"];
            var notionDatabaseId = _configuration["Notion:DatabaseId"];

            // Si la configuración no está establecida (valores por defecto), no sincronizar
            if (string.IsNullOrEmpty(notionSecret) || notionSecret.Contains("CONFIGURE_IN_APPSETTINGS") ||
                string.IsNullOrEmpty(notionDatabaseId) || notionDatabaseId.Contains("CONFIGURE_IN_APPSETTINGS"))
            {
                _logger.LogWarning("Notion sync is disabled: Configuration values not set. Please configure Notion:Secret and Notion:DatabaseId in appsettings.Development.json or user-secrets.");
                return;
            }

            var pollingSeconds = _configuration.GetValue<int>("Notion:PollingIntervalSeconds", 300);
            _logger.LogInformation("Notion sync worker started. Polling every {PollingSeconds} seconds.", pollingSeconds);

            // Esperar 10 segundos antes de la primera sincronización para permitir que la aplicación inicie correctamente
            try
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
            }
            catch (OperationCanceledException)
            {
                return;
            }

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
                    _logger.LogError(ex, "Error during Notion sync execution. Will retry in {PollingSeconds} seconds.", pollingSeconds);
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
