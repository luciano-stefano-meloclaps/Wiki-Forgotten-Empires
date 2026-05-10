using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Domain.Relations;
using Microsoft.Extensions.Logging;

namespace Infrastructure
{
    internal static class DbSeeder
    {
        public static void Seed(ApplicationContext context, ILogger logger)
        {
            if (context.Ages.Any())
            {
                logger.LogInformation("Database already contains data. Skipping seed.");
                return;
            }

            logger.LogInformation("Seeding initial data for SQLite database.");

            var ageAntigua = new Age
            {
                Name = "Edad Antigua",
                Date = "3000 a.C. - 476 d.C.",
                Overview = "Periodo marcado por la aparición de las primeras civilizaciones, la escritura y los grandes imperios.",
                Summary = "Primer gran ciclo histórico del ser humano con estados centralizados y desarrollo cultural."
            };

            var ageMedia = new Age
            {
                Name = "Edad Media",
                Date = "476 - 1492",
                Overview = "Epoca de feudalismo, cristianismo dominante y configuraciones políticas europeas.",
                Summary = "Transición entre el mundo antiguo y la Edad Moderna, con reinos, órdenes militares y universidades."
            };

            var ageModerna = new Age
            {
                Name = "Edad Moderna",
                Date = "1492 - 1789",
                Overview = "Periodo de exploraciones, reforma religiosa, absolutismo y los primeros pasos de la ciencia moderna.",
                Summary = "Transformación global con grandes descubrimientos geográficos y cambios sociopolíticos."
            };

            var roma = new Civilization
            {
                Name = "Imperio Romano",
                Overview = "Imperio clásico de la antigüedad que extendió su influencia por Europa, África y Asia.",
                Summary = "Potencia política y militar que dejó un legado en derecho, arquitectura y lengua.",
                State = CivilizationState.Dissolved,
                ImageUrl = null
            };

            var esparta = new Civilization
            {
                Name = "Esparta",
                Overview = "Ciudad-estado griega famosa por su disciplina militar y su sociedad guerrera.",
                Summary = "Potencia de la Hélade que defendió Grecia y luchó contra Atenas en la Guerra del Peloponeso.",
                State = CivilizationState.Dissolved,
                ImageUrl = null
            };

            var europa = new Territory
            {
                Name = "Europa"
            };

            var batallaAlesia = new Battle
            {
                Name = "Batalla de Alesia",
                Date = "52 a.C.",
                Summary = "Victoria decisiva de Julio César contra Vercingétorix durante la conquista de la Galia.",
                DetailedDescription = "El sitio de Alesia demostró la superioridad logística y estratégica de Roma frente a las tribus galas unidas.",
                Territory = TerritoryType.Europe,
                Age = ageAntigua
            };

            var julioCesar = new Character
            {
                Name = "Julio César",
                Description = "General, político y dictador romano que extendió enormemente el poder de Roma.",
                HonorificTitle = "Dictador Perpetuo",
                Dynasty = "Julio-Claudia",
                LifePeriod = "100 a.C. - 44 a.C.",
                Role = RoleCharacter.Emperor,
                Civilization = roma,
                Age = ageAntigua
            };

            var leonidas = new Character
            {
                Name = "Leónidas I",
                Description = "Rey de Esparta que lideró a los griegos en la famosa defensa de las Termópilas.",
                HonorificTitle = "Rey de Esparta",
                LifePeriod = "540 a.C. - 480 a.C.",
                Role = RoleCharacter.King,
                Civilization = esparta,
                Age = ageAntigua
            };

            var civAgeRome = new CivilizationAge
            {
                Civilization = roma,
                Age = ageAntigua
            };

            var civAgeSparta = new CivilizationAge
            {
                Civilization = esparta,
                Age = ageAntigua
            };

            var civTerritoryRome = new CivilizationTerritory
            {
                Civilization = roma,
                Territory = europa
            };

            var civTerritorySparta = new CivilizationTerritory
            {
                Civilization = esparta,
                Territory = europa
            };

            var civBattleRome = new CivilizationBattle
            {
                Civilization = roma,
                Battle = batallaAlesia
            };

            var civBattleSparta = new CivilizationBattle
            {
                Civilization = esparta,
                Battle = batallaAlesia
            };

            var characterBattleCesar = new CharacterBattle
            {
                Character = julioCesar,
                Battle = batallaAlesia,
                Outcome = ParticipantOutcome.Victory
            };

            var characterBattleLeonidas = new CharacterBattle
            {
                Character = leonidas,
                Battle = batallaAlesia,
                Outcome = ParticipantOutcome.Defeat
            };

            roma.Ages.Add(civAgeRome);
            esparta.Ages.Add(civAgeSparta);
            roma.Territories.Add(civTerritoryRome);
            esparta.Territories.Add(civTerritorySparta);
            batallaAlesia.Civilization.Add(civBattleRome);
            batallaAlesia.Civilization.Add(civBattleSparta);
            julioCesar.Battles.Add(characterBattleCesar);
            leonidas.Battles.Add(characterBattleLeonidas);

            context.AddRange(ageAntigua, ageMedia, ageModerna);
            context.AddRange(roma, esparta);
            context.AddRange(europa);
            context.AddRange(batallaAlesia);
            context.AddRange(julioCesar, leonidas);
            context.SaveChanges();

            logger.LogInformation("Initial SQLite data seeded successfully.");
        }
    }
}
