using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IBattleRepository
    {
        Task<IEnumerable<Battle>> GetAllBattles(CancellationToken ct);

        Task<Battle?> GetByIdBattle(int id, CancellationToken ct);

        Task<Battle> CreateBattle(Battle battle, CancellationToken ct);

        Task UpdateBattle(Battle battle, CancellationToken ct);

        Task DeleteBattle(Battle battle, CancellationToken ct);
    }
}