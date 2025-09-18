using Application.Interfaces;
using Application.Models.Request;
using Domain.Relations;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ForgottenEmpire.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgeController : ControllerBase
    {
        private readonly IAgeService _ageService;

        public AgeController(IAgeService ageService)
        {
            _ageService = ageService ?? throw new ArgumentNullException(nameof(ageService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(CancellationToken ct)
        {
            try
            {
                var ages = await _ageService.GetAllAges(ct);
                return Ok(ages);
            }
            catch (Exception)
            {
                return StatusCode(500, "Ocurrio un error al obtener las Edades.");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken ct)
        {
            try
            {
                var age = await _ageService.GetAgeById(id, ct);

                if (age == null)
                {
                    return NotFound($"No se encontró la Edad con id {id}");
                }
                return Ok(age);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al obtener la Edad: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Create(CreateAgeDto dto, CancellationToken ct)
        {
            try
            {
                var ageDto = await _ageService.CreateAge(dto, ct);
                return Ok(ageDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al crear la Edad: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAgeDto dto, CancellationToken ct)
        {
            try
            {
                var updatedAge = await _ageService.UpdateAge(id, dto, ct);
                if (!updatedAge)
                {
                    return NotFound($"No se encontró la Edad con id {id}");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al actualizar la Edad: {ex.Message}");
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken ct)
        {
            try
            {
                var deleteAge = await _ageService.DeleteAge(id, ct);
                if (!deleteAge)
                {
                    return NotFound($"No se encontró la Edad con id {id}");
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(500, new { mensaje = "Ocurrió un error inesperado." });
            }
        }

        /*[Authorize]
        [HttpPut("{ageId}/relations")]
        public async Task<IActionResult> UpdateRelations(int ageId, [FromBody] UpdateAgeRelationsDto dto, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.UpdateAgeRelations(ageId, dto, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al actualizar las relaciones de la Edad: {ex.Message}");
            }
        }*/

        /////// METODOS para VINCULAR relaciones por ID unico \\\\\\
        [Authorize]
        [HttpPut("{ageId}/battle")]
        public async Task<IActionResult> UpdateAgeBattleRelation(int ageId, [FromQuery] int battleId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.UpdateAgeBattleRelation(ageId, battleId, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return Ok(new { ageId, battleId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al actualizar la relación de la Edad con la Batalla: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPut("{ageId}/character")]
        public async Task<IActionResult> UpdateAgeCharacterRelation(int ageId, [FromQuery] int characterId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.UpdateAgeCharacterRelation(ageId, characterId, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return Ok(new { ageId, characterId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrio un error al actualizar la relación de la Edad con el Personaje: {ex.Message}");
            }
        }

        [HttpPut("{ageId}/civilization")]
        public async Task<IActionResult> UpdateAgeCivilizationRelation(int ageId, [FromQuery] int civilizationId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.UpdateAgeCivilizationRelation(ageId, civilizationId, ct);

                if (!success)
                {
                    // Si el mensaje indica que no se encontró algo, devolvemos NotFound.
                    if (errorMessage.Contains("No se encontró"))
                    {
                        return NotFound(new { message = errorMessage });
                    }

                    return BadRequest(new { message = errorMessage });
                }

                return Ok(new { ageId, civilizationId });
            }
            catch (Exception ex)
            {
                // Este catch es para errores verdaderamente inesperados en el controlador.
                return StatusCode(500, $"Ocurrió un error de servidor no controlado: {ex.Message}");
            }
        }

        /* [Authorize]
         [HttpDelete("{ageId}/relations")]
         public async Task<IActionResult> RemoveRelations(int ageId, [FromBody] UpdateAgeRelationsDto dto, CancellationToken ct)
         {
             try
             {
                 var (success, errorMessage) = await _ageService.RemoveAgeRelationsAsync(ageId, dto, ct);

                 if (!success)
                 {
                     return NotFound(new { message = errorMessage });
                 }

                 return NoContent();
             }
             catch (Exception ex)
             {
                 return StatusCode(500, $"Ocurrio un error al eliminar las relaciones de la Edad: {ex.Message}");
             }
         }*/

        /////// METODOS para DESVINCULAR relaciones por ID unico \\\\\\
        [Authorize]
        [HttpDelete("{ageId}/battle")]
        public async Task<IActionResult> RemoveAgeBattleRelation(int ageId, [FromQuery] int battleId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.RemoveAgeBattleRelation(ageId, battleId, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return Ok(new { ageId, battleId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la relación de la Edad con la Batalla: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{ageId}/character")]
        public async Task<IActionResult> RemoveAgeCharacterRelation(int ageId, [FromQuery] int characterId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.RemoveAgeCharacterRelation(ageId, characterId, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return Ok(new { ageId, characterId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la relación de la Edad con el Personaje: {ex.Message}");
            }
        }

        [Authorize]
        [HttpDelete("{ageId}/civilization")]
        public async Task<IActionResult> RemoveAgeCivilizationRelation(int ageId, [FromQuery] int civilizationId, CancellationToken ct)
        {
            try
            {
                var (success, errorMessage) = await _ageService.RemoveAgeCivilizationRelation(ageId, civilizationId, ct);

                if (!success)
                {
                    return NotFound(new { message = errorMessage });
                }

                return Ok(new { ageId, civilizationId });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ocurrió un error al eliminar la relación de la Edad con la Civilización: {ex.Message}");
            }
        }
    }
}