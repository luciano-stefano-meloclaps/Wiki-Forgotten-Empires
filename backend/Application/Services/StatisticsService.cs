using Application.Interfaces;
using Application.Models.Dto;
using Domain.Interfaces;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services
{
    public class StatisticsService : IStatisticsService
    {
        private readonly INotionDataStore _notionStore;

        public StatisticsService(INotionDataStore notionStore)
        {
            _notionStore = notionStore ?? throw new ArgumentNullException(nameof(notionStore));
        }

        public Task<GlobalStatsDto> GetGlobalStatsAsync(CancellationToken ct)
        {
            if (!_notionStore.IsInitialized)
            {
                throw new InvalidOperationException("Los datos de Notion no están disponibles. Por favor, sincronice primero.");
            }

            var stats = new GlobalStatsDto
            {
                TotalBattles = _notionStore.GetBattles().Count(),
                TotalCharacters = _notionStore.GetCharacters().Count(),
                TotalCivilizations = _notionStore.GetCivilizations().Count(),
                TotalAges = _notionStore.GetAges().Count()
            };

            return Task.FromResult(stats);
        }
    }
}
