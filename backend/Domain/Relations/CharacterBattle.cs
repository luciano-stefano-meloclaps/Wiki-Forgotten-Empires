using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities;
using Domain.Enums;

namespace Domain.Relations
{
    public class CharacterBattle
    {
        //Tabla con Ent. Battle
        [ForeignKey("Battle")]
        public int BattleId { get; set; }

        public Battle Battle { get; set; } = new Battle();

        //Tabla con Ent. Character
        [ForeignKey("Character")]
        public int CharacterId { get; set; }

        public Character Character { get; set; } = new Character();

        //public string FactionName { get; set; } Pensar en como implementar la facción, ya que puede ser por paises o pj

        public ParticipantOutcome Outcome { get; set; } = ParticipantOutcome.Unknown;
    }
}