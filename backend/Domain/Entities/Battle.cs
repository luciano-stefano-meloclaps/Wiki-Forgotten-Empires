using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Relations;

namespace Domain.Entities
{
    public class Battle
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "El campo Name es obligatorio.")]
        public string Name { get; set; } = default!;

        public string? DetailedDescription { get; set; }
        public string? Summary { get; set; }
        public string? Date { get; set; }

        //Enums
        public TerritoryType? Territory { get; set; }

        // Relacion N->N con Character
        public ICollection<CharacterBattle> Character { get; set; } = new List<CharacterBattle>();

        // Relacion N->N con Civilization
        public ICollection<CivilizationBattle> Civilization { get; set; } = new List<CivilizationBattle>();

        //Relacion N->1 con Age
        [ForeignKey("Age")]
        public int? AgeId { get; set; }

        public Age? Age { get; set; }
    }
}