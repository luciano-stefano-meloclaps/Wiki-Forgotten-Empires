using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Relations;

namespace Domain.Entities
{
    public class Civilization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string? Summary { get; set; }
        public string? Overview { get; set; }
        public string? ImageUrl { get; set; }

        //Enums
        public TerritoryType Territory { get; set; }

        public CivilizationState State { get; set; }

        //Relacion 1->N
        public ICollection<Character> Characters { get; set; }
            = new List<Character>();

        //Relacion N->N con Age
        public ICollection<CivilizationAge> Ages { get; set; }
            = new List<CivilizationAge>();

        //Relaciones N->N con Battle
        public ICollection<CivilizationBattle> Battles { get; set; }
            = new List<CivilizationBattle>();
    }
}