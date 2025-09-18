using System.Text.Json.Serialization;
using Domain.Entities;
using Domain.Enums;

namespace Application.Models.Dto
{
    public class CharacterDtoCard
    {
        public int Id { get; set; }
        public string Name { get; set; } = default!;
        public string? HonorificTitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? LifePeriod { get; set; }

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleCharacter? Role { get; set; }

        public static CharacterDtoCard ToDto(Character character)
        {
            return new CharacterDtoCard
            {
                Id = character.Id,
                Name = character.Name,
                HonorificTitle = character.HonorificTitle,
                ImageUrl = character.ImageUrl,
                LifePeriod = character.LifePeriod,
                Role = character.Role,
            };
        }
    }

    public class CharacterDtoDetail
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? HonorificTitle { get; set; }
        public string? ImageUrl { get; set; }
        public string? LifePeriod { get; set; }
        public string? Dynasty { get; set; }
        public string? Description { get; set; }

        //Enums
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoleCharacter? Role { get; set; }

        // Relacion 1--N
        public CivilizationGalleryDto? Civilization { get; set; }

        public AgeAccordionDto? Age { get; set; }

        //Relacion N-N con Battle
        public List<BattleTableDto> Battles { get; set; } = new();

        public static CharacterDtoDetail ToDto(Character character)
        {
            return new CharacterDtoDetail
            {
                Id = character.Id,
                Name = character.Name,
                HonorificTitle = character.HonorificTitle,
                ImageUrl = character.ImageUrl,
                LifePeriod = character.LifePeriod,
                Dynasty = character.Dynasty,
                Role = character.Role,
                Description = character.Description,
                //Relaciones
                Civilization = character.Civilization != null ? CivilizationGalleryDto.ToDto(character.Civilization) : null,
                Age = character.Age != null ? AgeAccordionDto.ToDto(character.Age) : null,
                Battles = character.Battles.Select(cb => BattleTableDto.ToDto(cb.Battle)).ToList()
            };
        }
    }
}