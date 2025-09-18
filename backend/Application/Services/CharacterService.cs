using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Interfaces;

namespace Application.Services
{
    public class CharacterService : ICharacterService
    {
        private readonly ICharacterRepository _characterRepository;

        public CharacterService(ICharacterRepository characterRepository)
        {
            _characterRepository = characterRepository ?? throw new ArgumentNullException(nameof(characterRepository));
        }

        public async Task<IEnumerable<CharacterDtoCard>> GetAllCharacters(CancellationToken ct)
        {
            var characters = await _characterRepository.GetAllCharacters(ct);
            return characters.Select(CharacterDtoCard.ToDto);
        }

        public async Task<CharacterDtoDetail?> GetCharacterById(int id, CancellationToken ct)
        {
            var character = await _characterRepository.GetCharacterById(id, ct);
            return character is null ? null : CharacterDtoDetail.ToDto(character);
        }

        public async Task<CharacterDtoDetail> CreateCharacter(CharacterCreateRequest request, CancellationToken ct)
        {
            var character = CharacterCreateRequest.ToEntity(request);
            await _characterRepository.CreateCharacter(character, ct);
            return CharacterDtoDetail.ToDto(character);
        }

        public async Task<CharacterDtoDetail?> UpdateCharacter(int id, CharacterUpdateRequest request, CancellationToken ct)
        {
            var character = await _characterRepository.GetCharacterById(id, ct);
            if (character is null)
            {
                return null;
            }
            CharacterUpdateRequest.ApplyToEntity(request, character);
            await _characterRepository.UpdateCharacter(character, ct);
            return CharacterDtoDetail.ToDto(character);
        }

        public async Task<bool> DeleteCharacter(int id, CancellationToken ct)
        {
            var character = await _characterRepository.GetCharacterById(id, ct);
            if (character is null)
            {
                return false;
            }
            await _characterRepository.DeleteCharacter(character, ct);
            return true;
        }
    }
}