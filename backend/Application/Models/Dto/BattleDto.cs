using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Dto
{
    public class BattleTableDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? Date { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public TerritoryType? Territory { get; set; }

        public static BattleTableDto ToDto(Battle battle)
        {
            return new BattleTableDto
            {
                Id = battle.Id,
                Date = battle.Date,
                Name = battle.Name,
                Territory = battle.Territory,
            };
        }

        public class BattleDetailDto
        {
            public int Id { get; set; }
            public string Name { get; set; } = default!;
            public string? DetailedDescription { get; set; }
            public string? Summary { get; set; }
            public string? Date { get; set; }

            //Enum
            [JsonConverter(typeof(JsonStringEnumConverter))]
            public TerritoryType? Territory { get; set; }

            // Relación 1--N
            public AgeAccordionDto? Age { get; set; }

            // Relación N--M
            public List<CharacterDtoCard> Characters { get; set; } = new();

            public List<CivilizationGalleryDto> Civilizations { get; set; } = new();

            public static BattleDetailDto ToDto(Battle battle)
            {
                return new BattleDetailDto
                {
                    Id = battle.Id,
                    Name = battle.Name,
                    DetailedDescription = battle.DetailedDescription,
                    Summary = battle.Summary,
                    Date = battle.Date,
                    Territory = battle.Territory,

                    // Mapeo con Age (si existe)
                    Age = battle.Age != null ? AgeAccordionDto.ToDto(battle.Age) : null,

                    // Mapeo de la colección N-M de Characters
                    Characters = battle.Character.Select(cb => CharacterDtoCard.ToDto(cb.Character)).ToList(),

                    Civilizations = battle.Civilization.Select(cb => CivilizationGalleryDto.ToDto(cb.Civilization)).ToList()
                };
            }
        }
    }
}