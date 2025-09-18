using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ForgottenEmpire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CivilizationController : ControllerBase
    {
        private readonly ICivilizationService _civilizationService;

        public CivilizationController(ICivilizationService civilizationService)
        {
            _civilizationService = civilizationService ?? throw new ArgumentNullException(nameof(civilizationService));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CivilizationGalleryDto>>> GetCivilizationsAll(CancellationToken ct)
        {
            try
            {
                var civilizations = await _civilizationService.GetAllCivilization(ct);
                return Ok(civilizations);
            }
            catch (Exception ex) //Dejo el error para ver por si pasa algo. Sino scar
            {
                return StatusCode(500, $"Ocurrio un error al obtener las Civilizaciones: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CivilizationDetailDto>> GetCivilizationById(int id, CancellationToken ct)
        {
            try
            {
                var civilizationDetail = await _civilizationService.GetCivizlizationById(id, ct);

                if (civilizationDetail == null)
                {
                    return NotFound($"No se encontró la Civilización con id {id}.");
                }
                return Ok(civilizationDetail);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al obtener las Civilizaciones: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<CivilizationDetailDto>> CreateCivilization(CreateCivilizationRequest request, CancellationToken ct)
        {
            try
            {
                var civilizationDto = await _civilizationService.CreateCivilization(request, ct); //Ver como devovler 201 segun normas REST
                return Ok(civilizationDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al obtener las Civilizaciones: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCivilization(int id, [FromBody] UpdateCivilizationRequest request, CancellationToken ct)
        {
            try
            {
                var civilizationSuccess = await _civilizationService.UpdateCivilization(id, request, ct);

                if (!civilizationSuccess)
                {
                    return NotFound($"No se encontró la civilización con id {id}.");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al obtener las Civilizaciones: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCivilization(int id, CancellationToken ct)
        {
            try
            {
                var civilizationSuccess = await _civilizationService.DeleteCivilization(id, ct);

                if (!civilizationSuccess)
                {
                    return NotFound($"No se encontró la civilización con id {id}");
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error inesperado al eliminar la Civilización: {ex.Message}");
            }
        }
    }
}