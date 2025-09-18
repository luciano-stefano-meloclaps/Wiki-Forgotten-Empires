using System.ComponentModel.DataAnnotations;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Request
{
    public class BattleCreateRequest
    {
        [MinLength(10, ErrorMessage = "El campo Nombre debe tener al menos 10 caracteres.")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo Nombre es obligatorio.")]
        public string Name { get; set; } = default!;

        public string? Date { get; set; }

        public static Battle ToEntity(BattleCreateRequest dto)
        {
            return new Battle
            {
                Name = dto.Name,
                Date = dto.Date,
            };
        }
    }

    public class BattleUpdateRequest
    {
        [MinLength(10, ErrorMessage = "El campo Nombre debe tener al menos 10 caracteres.")]
        public string? Name { get; set; }

        public string? Summary { get; set; }
        public string? DetailedDescription { get; set; }
        public string? Date { get; set; }
        public TerritoryType? Territory { get; set; }

        //Relaciones
        //public int? AgeId { get; set; }

        //public List<int>? CharactersIds { get; set; }
        //public List<int>? CivilizationsIds { get; set; }

        public static void ApplyToEntity(BattleUpdateRequest dto, Battle battle)
        {
            if (dto.Name is not null) battle.Name = dto.Name;
            if (dto.Summary is not null) battle.Summary = dto.Summary;
            if (dto.DetailedDescription is not null) battle.DetailedDescription = dto.DetailedDescription;
            if (dto.Date is not null) battle.Date = dto.Date;
            if (dto.Territory is not null) battle.Territory = dto.Territory;
            //if (dto.AgeId.HasValue) battle.AgeId = dto.AgeId.Value;
        }
    }
}