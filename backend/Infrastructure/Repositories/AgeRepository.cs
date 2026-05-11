using Domain.Interfaces;
using Domain.Entities;
using Domain.Relations;

namespace Infrastructure.Repositories
{
    public class AgeRepository : IAgeRepository
    {
        private readonly INotionDataStore _notionStore;

        public AgeRepository(INotionDataStore notionStore)
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

        public async Task<List<Age>> GetAllAges(CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetAges().ToList();
        }

        public async Task<Age?> GetAgeDetailById(int id, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetAgeById(id);
        }

        public async Task<Age?> GetTrackedAgeById(int id, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetAgeById(id);
        }

        public async Task<Age> CreateAge(Age age, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite crear edades desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task<Age?> GetAgeByName(string name, CancellationToken ct)
        {
            return _notionStore.GetAges().FirstOrDefault(a => a.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateAge(Age age, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite actualizar edades desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task DeleteAge(Age age, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite eliminar edades desde la API. Los datos deben gestionarse en Notion.");
        }

        /////// METODOS para VINCULAR relaciones por ID unico \\\\\\
        public async Task<bool> LinkBattleAsync(int ageId, int battleId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }

        public async Task<bool> LinkCharacterAsync(int ageId, int characterId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }

        public async Task<bool> LinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }

        /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
        public async Task<bool> UnlinkBattleAsync(int ageId, int battleId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }

        public async Task<bool> UnlinkCharacterAsync(int ageId, int characterId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }

        public async Task<bool> UnlinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct)
        {
            throw new NotImplementedException("Relations are managed by Notion sync. Update data directly in Notion database.");
        }
    }
}