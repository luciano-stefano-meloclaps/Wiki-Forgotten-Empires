using Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize(Roles = "Admin")] // 🔒 SEGURIDAD: Solo administradores pueden ejecutar sync
        public async Task<IActionResult> Sync(CancellationToken ct)
        {
            try
            {
                await _notionSyncService.SyncFromNotionAsync(ct);
                return Ok(new { message = "Notion sync ejecutado exitosamente.", timestamp = DateTime.UtcNow });
            }
            catch (Exception ex)
            {
                // ⚠️  SEGURIDAD: No exponer stack trace en producción
                var error = ex.InnerException?.Message ?? ex.Message;
                return StatusCode(500, new { error });
            }
        }

        [HttpGet("health")]
        public IActionResult Health()
        {
            return Ok(new { status = "Notion sync ready" });
        }
    }
}
