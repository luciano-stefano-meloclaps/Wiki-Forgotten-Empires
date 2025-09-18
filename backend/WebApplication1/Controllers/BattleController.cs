using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static Application.Models.Dto.BattleTableDto;

namespace ForgottenEmpire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BattleController : ControllerBase
    {
        private readonly IBattleService _battleService;

        public BattleController(IBattleService battleService)
        {
            _battleService = battleService ?? throw new ArgumentNullException(nameof(battleService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BattleTableDto>>> Get(CancellationToken ct)
        {
            try
            {
                var battlesDetail = await _battleService.GetAllBattles(ct);
                return Ok(battlesDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener las Batallas: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BattleDetailDto>> GetBattleDetailById(int id, CancellationToken ct)
        {
            try
            {
                var battleDetail = await _battleService.GetByIdBattle(id, ct);
                if (battleDetail == null)
                    return NotFound($"No se encontro la Batalla con ID: {id}");

                return Ok(battleDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al obtener la Batallas: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<BattleDetailDto>> Create([FromBody] BattleCreateRequest request, CancellationToken ct)
        {
            try
            {
                var battleDto = await _battleService.CreateBattle(request, ct);
                return Ok(battleDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al crear las Batallas: {ex.GetType().Name}{ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, [FromBody] BattleUpdateRequest request, CancellationToken ct)
        {
            try
            {
                var battleSuccess = await _battleService.UpdateBattle(id, request, ct);
                if (battleSuccess is null)
                    return NotFound($"No se encontró la Batalla con ID {id}");

                await _battleService.UpdateBattle(id, request, ct);
                return NoContent(); //204 sin contenido para el payload
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al modificar la Batalla: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                var battleSuccess = await _battleService.DeleteBattle(id, ct);
                if (!battleSuccess)
                {
                    return NotFound($"No se encontró la Batalla con ID {id}");
                }
                return NoContent(); //204 (vacio)
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la Batalla: {ex.Message}");
            }
        }
    }
}