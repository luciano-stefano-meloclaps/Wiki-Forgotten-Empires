using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Relations;

namespace Domain.Entities
{
    public class Age
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo 'Nombre' es obligatorio para registrar una nueva Edad.")]
        public string Name { get; set; } = default!;

        public string? Summary { get; set; } //Description
        public string? Date { get; set; }
        public string? Overview { get; set; }

        //Para seteear las relaciones
        //public int? BattleId { get; set; }

        //public int? CharacterId { get; set; }

        //Relaciones 1->N
        public ICollection<Character> Characters { get; set; } = new List<Character>();

        public ICollection<Battle> Battles { get; set; } = new List<Battle>();

        // / Relacion N->N con Civilization
        public ICollection<CivilizationAge> Civilizations { get; set; } = new List<CivilizationAge>();
    }
}