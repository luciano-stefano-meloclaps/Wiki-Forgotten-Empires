using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Dto
{
    public class CivilizationGalleryDto
    {
        public int Id { get; set; }

        public string Name { get; set; } = default!;

        public string? Summary { get; set; }

        public string? ImageUrl { get; set; }

        public List<string> Territories { get; set; } = new();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CivilizationState State { get; set; }

        public int AgesCount { get; set; }
        public int CharactersCount { get; set; }
        public int BattlesCount { get; set; }

        public static CivilizationGalleryDto ToDto(Civilization civilization)
        {
            return new CivilizationGalleryDto
            {
                Id = civilization.Id,
                Name = civilization.Name,
                Summary = civilization.Summary,
                ImageUrl = civilization.ImageUrl,
                Territories = civilization.Territories.Select(ct => ct.Territory.Name).ToList(),
                State = civilization.State,
                AgesCount = civilization.Ages.Count,
                CharactersCount = civilization.Characters.Count,
                BattlesCount = civilization.Battles.Count
            };
        }
    }

    public class CivilizationDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Overview { get; set; }
        public string? ImageUrl { get; set; }

        public List<string> Territories { get; set; } = new();

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public CivilizationState? State { get; set; }

        //Relacion N--N
        public List<AgeAccordionDto>? Ages { get; set; } = new();

        public List<CharacterDtoCard> Characters { get; set; } = new();
        public List<BattleTableDto> Battles { get; set; } = new();

        public static CivilizationDetailDto ToDto(Civilization civilization)
        {
            return new CivilizationDetailDto
            {
                Id = civilization.Id,
                Name = civilization.Name,
                Overview = civilization.Overview,
                ImageUrl = civilization.ImageUrl,
                Territories = civilization.Territories.Select(ct => ct.Territory.Name).ToList(),
                State = civilization.State,
                //Relaciones
                Characters = civilization.Characters.Select(c => CharacterDtoCard.ToDto(c)).ToList(),
                Ages = civilization.Ages
                    .Where(a => a.Age != null)
                    .Select(a => AgeAccordionDto.ToDto(a.Age!))
                    .ToList(),
                Battles = civilization.Battles.Select(b => BattleTableDto.ToDto(b.Battle)).ToList()
            };
        }
    }
}