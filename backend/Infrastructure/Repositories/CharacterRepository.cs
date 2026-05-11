using Domain.Entities;
using Domain.Interfaces;

namespace Infrastructure.Repositories
{
    public class CharacterRepository : ICharacterRepository
    {
        private readonly INotionDataStore _notionStore;

        public CharacterRepository(INotionDataStore notionStore)
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

        public async Task<IEnumerable<Character>> GetAllCharacters(CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCharacters().ToList();
        }

        public async Task<Character?> GetCharacterById(int id, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCharacterById(id);
        }

        public async Task<Character> CreateCharacter(Character character, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite crear personajes desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task<Character?> GetCharacterByName(string name, CancellationToken ct)
        {
            EnsureInitialized();
            return _notionStore.GetCharacters().FirstOrDefault(c => c.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public async Task UpdateCharacter(Character character, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite actualizar personajes desde la API. Los datos deben gestionarse en Notion.");
        }

        public async Task DeleteCharacter(Character character, CancellationToken ct)
        {
            throw new NotSupportedException("No se permite eliminar personajes desde la API. Los datos deben gestionarse en Notion.");
        }
    }
}