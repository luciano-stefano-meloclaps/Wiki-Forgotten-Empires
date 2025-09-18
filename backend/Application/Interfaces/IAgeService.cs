using Application.Models.Dto;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface IAgeService
    {
        Task<IEnumerable<AgeAccordionDto>> GetAllAges(CancellationToken ct);

        Task<AgeDetailDto?> GetAgeById(int id, CancellationToken ct);

        Task<AgeDetailDto> CreateAge(CreateAgeDto request, CancellationToken ct);

        Task<bool> UpdateAge(int id, UpdateAgeDto request, CancellationToken ct);

        Task<bool> DeleteAge(int id, CancellationToken ct);

        // Task<(bool Success, string ErrorMessage)> UpdateAgeRelations(int id, UpdateAgeRelationsDto dto, CancellationToken ct);


        //Task<(bool Success, string ErrorMessage)> RemoveAgeRelationsAsync(int ageId, UpdateAgeRelationsDto dto, CancellationToken ct);

        /////// METODOS para VINCULAR relaciones por ID unico \\\\\\
        Task<(bool Success, string ErrorMessage)> UpdateAgeBattleRelation(int ageId, int battleId, CancellationToken ct);

        Task<(bool Success, string ErrorMessage)> UpdateAgeCharacterRelation(int ageId, int characterId, CancellationToken ct);

        Task<(bool Success, string ErrorMessage)> UpdateAgeCivilizationRelation(int ageId, int civilizationId, CancellationToken ct);

        /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
        Task<(bool Success, string ErrorMessage)> RemoveAgeBattleRelation(int ageId, int battleId, CancellationToken ct);

        Task<(bool Success, string ErrorMessage)> RemoveAgeCharacterRelation(int ageId, int characterId, CancellationToken ct);

        Task<(bool Success, string ErrorMessage)> RemoveAgeCivilizationRelation(int ageId, int civilizationId, CancellationToken ct);
    }
}