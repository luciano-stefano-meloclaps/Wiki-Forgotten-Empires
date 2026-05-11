using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class BatlleRepository : IBattleRepository
    {
        private readonly INotionDataStore _notionStore;

        public BatlleRepository(INotionDataStore notionStore)
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

        public async Task<IEnumerable<Battle>> GetAllBattles(CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetBattles().ToList();
        }

        public async Task<Battle?> GetByIdBattle(int id, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetBattleById(id);
        }

        public async Task<Battle> CreateBattle(Battle battle, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite crear batallas desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task<Battle?> GetBattleByName(string name, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetBattles().FirstOrDefault(b => b.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateBattle(Battle battle, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite actualizar batallas desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task DeleteBattle(Battle battle, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite eliminar batallas desde la API. Los datos deben gestionarse en Notion.");
        }
    }
}