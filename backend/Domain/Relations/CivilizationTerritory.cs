using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;

namespace Domain.Relations
{
    public class CivilizationTerritory
    {
        [ForeignKey("Territory")]
        public int TerritoryId { get; set; }

        public Territory Territory { get; set; } = new Territory();

        [ForeignKey("Civilization")]
        public int CivilizationId { get; set; }

        public Civilization Civilization { get; set; } = new Civilization();
    }
}
