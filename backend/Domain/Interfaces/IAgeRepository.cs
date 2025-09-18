using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAgeRepository
    {
        Task<List<Age>> GetAllAges(CancellationToken ct);

        Task<Age?> GetAgeDetailById(int id, CancellationToken ct);

        Task<Age?> GetTrackedAgeById(int id, CancellationToken ct);

        //Task<Battle?> GetBattleById(int battleId, CancellationToken ct);

        Task<Age> CreateAge(Age age, CancellationToken ct);

        Task UpdateAge(Age ageFromRequest, CancellationToken ct);

        Task DeleteAge(Age age, CancellationToken ct);

        /////// METODOS para VINCULAR relaciones por ID unico \\\\\\
        Task<bool> LinkBattleAsync(int ageId, int battleId, CancellationToken ct);

        Task<bool> LinkCharacterAsync(int ageId, int characterId, CancellationToken ct);

        Task<bool> LinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct);

        //Task RemoveBattleRelation(Battle battle, CancellationToken ct);

        /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
        Task<bool> UnlinkBattleAsync(int ageId, int battleId, CancellationToken ct);

        Task<bool> UnlinkCharacterAsync(int ageId, int characterId, CancellationToken ct);

        Task<bool> UnlinkCivilizationAsync(int ageId, int civilizationId, CancellationToken ct);
    }
}