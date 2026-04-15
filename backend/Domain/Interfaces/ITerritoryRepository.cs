using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ITerritoryRepository
    {
        Task<Territory?> GetTerritoryByName(string name, CancellationToken ct);

        Task<List<Territory>> GetTerritoriesByNames(IEnumerable<string> names, CancellationToken ct);

        Task<Territory> CreateTerritory(Territory territory, CancellationToken ct);
    }
}
