using System.ComponentModel.DataAnnotations;
using Domain.Entities;

namespace Application.Models.Dto
{
    public class AgeAccordionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Summary { get; set; }
        public string? Date { get; set; }

        public static AgeAccordionDto ToDto(Age age)
        {
            return new AgeAccordionDto
            {
                Id = age.Id,
                Name = age.Name,
                Summary = age.Summary,
                Date = age.Date
            };
        }
    }

    public class AgeDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Date { get; set; }
        public string? Overview { get; set; }
        public string? Summary { get; set; }

        // Relacion 1--N
        public List<BattleTableDto> Battles { get; set; } = new();

        // Relacion N--M
        public List<CharacterDtoCard> Characters { get; set; } = new();

        public List<CivilizationGalleryDto> Civilizations { get; set; } = new();

        public static AgeDetailDto ToDto(Age age)
        {
            return new AgeDetailDto
            {
                Id = age.Id,
                Name = age.Name,
                Summary = age.Summary,
                Date = age.Date,
                Overview = age.Overview,
                //Relaciones
                Battles = age.Battles.Select(BattleTableDto.ToDto).ToList(),
                Characters = age.Characters.Select(CharacterDtoCard.ToDto).ToList(),
                //Si da NULL hacer un where sino !
                Civilizations = age.Civilizations.Select(ca => CivilizationGalleryDto.ToDto(ca.Civilization)).ToList()
            };
        }
    }
}