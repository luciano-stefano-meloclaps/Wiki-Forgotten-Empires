using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CivilizationRepository : ICivilizationRepository
    {
        private readonly INotionDataStore _notionStore;

        public CivilizationRepository(INotionDataStore notionStore)
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

        public async Task<IEnumerable<Civilization>> GetAllCivilization(CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCivilizations().ToList();
        }

        public async Task<Civilization?> GetCivilizationById(int id, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCivilizationById(id);
        }

        public async Task<Civilization?> GetCivilizationByName(string name, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCivilizations().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<Civilization> CreateCivilization(Civilization civilization, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite crear civilizaciones desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task UpdateCivilization(Civilization civilization, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite actualizar civilizaciones desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task DeleteCivilization(Civilization civilization, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite eliminar civilizaciones desde la API. Los datos deben gestionarse en Notion.");
        }
    }
}