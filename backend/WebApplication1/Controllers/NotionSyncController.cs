using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ForgottenEmpire.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotionSyncController : ControllerBase
    {
        private readonly INotionSyncService _notionSyncService;

        public NotionSyncController(INotionSyncService notionSyncService)
        {
            _notionSyncService = notionSyncService ?? throw new ArgumentNullException(nameof(notionSyncService));
        }

        [HttpPost("sync")]
        public async Task<IActionResult> Sync(CancellationToken ct)
        {
            try
            {
                await _notionSyncService.SyncFromNotionAsync(ct);
                return Ok(new { message = "Notion sync ejecutado." });
            }
            catch (Exception ex)
            {
                var error = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { error, stackTrace = ex.ToString() });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Notion sync ready" });
        }
    }
}
