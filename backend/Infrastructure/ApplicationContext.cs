using Domain.Entities;
using Domain.Relations;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure
{
    public class ApplicationContext : DbContext
    { // EF usa estos DbSet<> para mapear ent. -> tablas y relacionar
        //Tablas principales

        public DbSet<Character> Characters { get; set; }
        public DbSet<Civilization> Civilizations { get; set; }
        public DbSet<Battle> Battles { get; set; }
        public DbSet<Age> Ages { get; set; }

        //Tablas de relaciones N->N
        public DbSet<CharacterBattle> CharacterBattles { get; set; }

        public DbSet<CivilizationBattle> CivilizationBattles { get; set; }

        public DbSet<CivilizationAge> CivilizationAges { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
            // Constructor de la clase ApplicationContext que recibe las opciones de DbContext
            // y un booleano para indicar si es un entorno de prueba.
            // Al registrarlo le psasamos las opciones de conexion
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //////////////////////Relaciones Uno a Muchos 1--N////////////////////////////

            //Age 1--N Battle
            modelBuilder.Entity<Battle>()
                .HasOne(battle => battle.Age)
                .WithMany(age => age.Battles)
                .HasForeignKey(battle => battle.AgeId)
                .OnDelete(DeleteBehavior.Restrict);

            //Civilization 1--N Character
            modelBuilder.Entity<Character>()
                .HasOne(character => character.Civilization)
                .WithMany(civilization => civilization.Characters)
                .HasForeignKey(character => character.CivilizationId)
                .OnDelete(DeleteBehavior.Restrict);

            //Age 1--N Character
            modelBuilder.Entity<Character>()
                .HasOne(character => character.Age)
                .WithMany(age => age.Characters)
                .HasForeignKey(character => character.AgeId)
                .OnDelete(DeleteBehavior.Restrict);

            //////////////////Relaciones Muchos a Muchos N--N///////////////////////////

            //Character N--N Battle
            modelBuilder.Entity<CharacterBattle>()
                //clave primaria compuesta. Asi un personaje no se repit
                .HasKey(cb => new { cb.CharacterId, cb.BattleId });

            //CharacterBattle --> Character
            modelBuilder.Entity<CharacterBattle>()
                .HasOne(cb => cb.Character)
                .WithMany(character => character.Battles)
                .HasForeignKey(cb => cb.CharacterId)
                .OnDelete(DeleteBehavior.Cascade);

            //CharacterBattle --> Battle
            modelBuilder.Entity<CharacterBattle>()
                .HasOne(cb => cb.Battle)
                .WithMany(battle => battle.Character)
                .HasForeignKey(cb => cb.BattleId)
                .OnDelete(DeleteBehavior.Cascade);

            //Civilization N--N Battle
            modelBuilder.Entity<CivilizationBattle>()
                .HasKey(cb => new { cb.CivilizationId, cb.BattleId });

            //CivilizationBattle --> Civilization
            modelBuilder.Entity<CivilizationBattle>()
                .HasOne(cb => cb.Civilization)
                .WithMany(civilization => civilization.Battles)
                .HasForeignKey(cb => cb.CivilizationId)
                .OnDelete(DeleteBehavior.Cascade);

            //CivilizationBattle --> Battle
            modelBuilder.Entity<CivilizationBattle>()
                .HasOne(cb => cb.Battle)
                .WithMany(battle => battle.Civilization)
                .HasForeignKey(cb => cb.BattleId)
                .OnDelete(DeleteBehavior.Cascade);

            //Civilization N--N Age
            modelBuilder.Entity<CivilizationAge>()
                .HasKey(ca => new { ca.CivilizationId, ca.AgeId });

            //CivilizationAge --> Civilization
            modelBuilder.Entity<CivilizationAge>()
                .HasOne(ca => ca.Civilization)
                .WithMany(Civilization => Civilization.Ages)
                .HasForeignKey(ca => ca.CivilizationId)
                .OnDelete(DeleteBehavior.Cascade);

            //CivilizationAge --> Age
            modelBuilder.Entity<CivilizationAge>()
                .HasOne(ca => ca.Age)
                .WithMany(Age => Age.Civilizations)
                .HasForeignKey(ca => ca.AgeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}