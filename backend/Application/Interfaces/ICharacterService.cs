using Application.Models.Dto;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface ICharacterService
    {
        Task<IEnumerable<CharacterDtoCard>> GetAllCharacters(CancellationToken ct);

        Task<CharacterDtoDetail?> GetCharacterById(int id, CancellationToken ct);

        Task<CharacterDtoDetail> CreateCharacter(CharacterCreateRequest request, CancellationToken ct);

        Task<CharacterDtoDetail?> UpdateCharacter(int id, CharacterUpdateRequest request, CancellationToken ct);

        Task<bool> DeleteCharacter(int id, CancellationToken ct);
    }
}