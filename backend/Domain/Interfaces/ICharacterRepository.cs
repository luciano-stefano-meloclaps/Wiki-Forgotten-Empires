using Domain.Entities;

namespace Domain.Interfaces
{
    public interface ICharacterRepository
    {
        Task<IEnumerable<Character>> GetAllCharacters(CancellationToken ct);

        Task<Character?> GetCharacterById(int id, CancellationToken ct);

        Task<Character?> GetCharacterByName(string name, CancellationToken ct);

        // Se elimina FindByIdAsync para simplificar.

        // Se añade el método para crear que faltaba.
        Task<Character> CreateCharacter(Character character, CancellationToken ct);

        Task UpdateCharacter(Character character, CancellationToken ct);

        Task DeleteCharacter(Character character, CancellationToken ct);
    }
}