using Application.Interfaces;
using Application.Models.Dto;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ForgottenEmpire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService ?? throw new ArgumentNullException(nameof(statisticsService));
        }

        [HttpGet]
        public async Task<IActionResult> GetGlobalStats(CancellationToken ct)
        {
            try
            {
                var stats = await _statisticsService.GetGlobalStatsAsync(ct);
                return Ok(stats);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrio un error al obtener las estadísticas globales.");
            }
        }
    }
}
