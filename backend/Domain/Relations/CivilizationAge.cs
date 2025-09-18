using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Relations
{
    public class CivilizationAge
    {
        //Tabla con Ent. Age
        [ForeignKey("Age")]
        public int AgeId { get; set; }

        public Age? Age { get; set; }

        //Tabla con Ent. Civilization
        [ForeignKey("Civilization")]
        public int CivilizationId { get; set; }

        public Civilization? Civilization { get; set; }
    }
}