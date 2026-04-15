using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Relations;

namespace Domain.Entities
{
    public class Territory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; } = default!;

        public ICollection<CivilizationTerritory> Civilizations { get; set; } = new List<CivilizationTerritory>();
    }
}
