using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Enums;
using Domain.Relations;

namespace Domain.Entities
{
    public class Character
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public string? ImageUrl { get; set; }
        public string? Description { get; set; }
        public string? HonorificTitle { get; set; }
        public string? Dynasty { get; set; }

        // Proximamente sera añadido un campo para la fecha de nacimiento y muerte de los personajes
        //De esa manera se podra calcular la edad de los personajes
        public string? LifePeriod { get; set; }

        //Enum
        public RoleCharacter? Role { get; set; }

        //Relacion N<-1 con Civilization
        [ForeignKey("Civilization")]
        public int? CivilizationId { get; set; } //Los hice nulleabe por la autoreferencia ID del problema referencia. Una vez con personajes SACARLO

        public Civilization? Civilization { get; set; }

        // Relacion N<-1 con Age
        [ForeignKey("Age")]
        public int? AgeId { get; set; }

        public Age? Age { get; set; }

        // Relacion N->N con Battle
        public ICollection<CharacterBattle> Battles { get; set; } = new List<CharacterBattle>();
    }
}