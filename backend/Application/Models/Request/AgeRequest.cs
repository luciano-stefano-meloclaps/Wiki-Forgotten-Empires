using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Models.Request
{
    public class CreateAgeDto
    {
        [Required(ErrorMessage = "El campo Nombre es obligatorio.")]
        [MinLength(10, ErrorMessage = "El campo Nombre debe tener al menos 10 caracteres.")]
        public string Name { get; set; } = default!;

        [StringLength(150, ErrorMessage = "El campo Summary no debe superar los 150 caracteres.")]
        public string? Summary { get; set; }

        public string? Date { get; set; }

        public static Age ToEntity(CreateAgeDto dto)
        {
            return new Age
            {
                Name = dto.Name,
                Summary = dto.Summary,
                Date = dto.Date
            };
        }
    }

    public class UpdateAgeDto
    {
        [Required(ErrorMessage = "El campo 'Nombre' no puede quedar vacío al actualizar la información de la Edad.")]
        [MinLength(10, ErrorMessage = "El campo Nombre debe tener al menos 10 caracteres."),
        StringLength(50, ErrorMessage = "El campo Nombre no debe superar los 50 caracteres.")]
        public string? Name { get; set; }

        [StringLength(150, ErrorMessage = "El campo Summary no debe superar los 150 caracteres.")]
        public string? Summary { get; set; }

        public string? Date { get; set; }
        public string? Overview { get; set; }

        public static void ApplyToEntity(UpdateAgeDto dto, Age age)

        {
            if (dto.Name is not null) age.Name = dto.Name;
            if (dto.Summary is not null) age.Summary = dto.Summary;
            if (dto.Date is not null) age.Date = dto.Date;
            if (dto.Overview is not null) age.Overview = dto.Overview;
        }
    }

    public class UpdateAgeRelationsDto
    {
        public int? BattleId { get; set; }
        public int? CharacterId { get; set; }
        public int? CivilizationId { get; set; }
    }
}