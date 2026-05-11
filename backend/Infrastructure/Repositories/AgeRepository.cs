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

        public async Task<IEnumerable<Age>> GetAllAges(CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetAges();
        }

        public async Task<Age?> GetAgeById(int id, CancellationToken ct)
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

    }
}