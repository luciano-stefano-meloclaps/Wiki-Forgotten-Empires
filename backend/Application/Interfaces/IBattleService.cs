using Application.Models.Dto;
using Application.Models.Request;
using static Application.Models.Dto.BattleTableDto;

namespace Application.Interfaces
{
    public interface IBattleService
    {
        Task<IEnumerable<BattleTableDto>> GetAllBattles(CancellationToken ct);

        Task<BattleDetailDto?> GetByIdBattle(int id, CancellationToken ct);

        Task<BattleDetailDto> CreateBattle(BattleCreateRequest dto, CancellationToken ct);

        Task<BattleDetailDto?> UpdateBattle(int id, BattleUpdateRequest dto, CancellationToken ct);

        Task<bool> DeleteBattle(int id, CancellationToken ct);
    }
}