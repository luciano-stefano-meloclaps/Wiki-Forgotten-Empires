using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Interfaces;
using static Application.Models.Dto.BattleTableDto;

namespace Application.Services
{
    public class BattleService : IBattleService
    {
        private readonly IBattleRepository _battleRepository;

        public BattleService(IBattleRepository battleRepository)
        {
            _battleRepository = battleRepository ?? throw new ArgumentNullException(nameof(battleRepository));
        }

        public async Task<IEnumerable<BattleTableDto>> GetAllBattles(CancellationToken ct)
        {
            var battles = await _battleRepository.GetAllBattles(ct);
            return battles.Select(BattleTableDto.ToDto);
        }

        public async Task<BattleDetailDto?> GetByIdBattle(int id, CancellationToken ct)
        {
            var battle = await _battleRepository.GetByIdBattle(id, ct);
            return battle is null ? null : BattleDetailDto.ToDto(battle);
        }

        public async Task<BattleDetailDto> CreateBattle(BattleCreateRequest dto, CancellationToken ct)
        {
            var battle = BattleCreateRequest.ToEntity(dto);
            await _battleRepository.CreateBattle(battle, ct);
            return BattleDetailDto.ToDto(battle);
        }

        //DRY: Don't Repeat Yourself a diferncia de Age se usa un Helper para hacer DPE (Es el ToDto)
        public async Task<BattleDetailDto?> UpdateBattle(int id, BattleUpdateRequest request, CancellationToken ct)
        {
            var battle = await _battleRepository.GetByIdBattle(id, ct);
            if (battle is null)
            {
                return null;
            }

            BattleUpdateRequest.ApplyToEntity(request, battle);
            await _battleRepository.UpdateBattle(battle, ct);
            return BattleDetailDto.ToDto(battle);
        }

        public async Task<bool> DeleteBattle(int id, CancellationToken ct)
        {
            var battle = await _battleRepository.GetByIdBattle(id, ct);
            if (battle is null)
            {
                return false;
            }
            await _battleRepository.DeleteBattle(battle, ct);
            return true;
        }
    }
}