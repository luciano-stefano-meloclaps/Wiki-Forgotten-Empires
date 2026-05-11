using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class TerritoryRepository : ITerritoryRepository
    {
        private readonly INotionDataStore _notionStore;

        public TerritoryRepository(INotionDataStore notionStore)
        {
            _notionStore = notionStore ?? throw new ArgumentNullException(nameof(notionStore));
        }

        private void EnsureInitialized()
        {
            if (!_notionStore.IsInitialized)
            {
                throw new InvalidOperationException("Los datos de Notion no están disponibles. Por favor, sincronice primero.");
            }
        }

        public async Task<Territory?> GetTerritoryByName(string name, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetTerritories().FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<List<Territory>> GetTerritoriesByNames(IEnumerable<string> names, CancellationToken ct)
        {
            EnsureInitialized();
            var normalizedNames = names
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            return _notionStore.GetTerritories()
                .Where(t => normalizedNames.Contains(t.Name, StringComparer.OrdinalIgnoreCase))
                .ToList();
        }

        public async Task<Territory> CreateTerritory(Territory territory, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite crear territorios desde la API. Los datos deben gestionarse en Notion.");
        }
    }
}
