using Domain.Interfaces;
using Domain.Entities;
using Domain.Relations;
using System.Collections.Concurrent;

namespace Application.Services
{
    public class NotionDataStore : INotionDataStore
    {
        private readonly ConcurrentDictionary<int, Age> _ages = new();
        private readonly ConcurrentDictionary<int, Battle> _battles = new();
        private readonly ConcurrentDictionary<int, Character> _characters = new();
        private readonly ConcurrentDictionary<int, Civilization> _civilizations = new();
        private readonly ConcurrentDictionary<int, Territory> _territories = new();

        private readonly ConcurrentBag<CharacterBattle> _characterBattles = new();
        private readonly ConcurrentBag<CivilizationBattle> _civilizationBattles = new();
        private readonly ConcurrentBag<CivilizationAge> _civilizationAges = new();
        private readonly ConcurrentBag<CivilizationTerritory> _civilizationTerritories = new();

        public bool IsInitialized { get; private set; }
        public DateTime? LastSyncTime { get; private set; }

        public void MarkAsInitialized()
        {
            IsInitialized = true;
            LastSyncTime = DateTime.UtcNow;
        }

        // Ages
        public IEnumerable<Age> GetAges() => _ages.Values;
        public Age? GetAgeById(int id) => _ages.TryGetValue(id, out var age) ? age : null;

        public void UpdateAgesFromNotion(IEnumerable<Age> ages)
        {
            _ages.Clear();
            foreach (var age in ages)
            {
                _ages[age.Id] = age;
            }
        }

        // Battles
        public IEnumerable<Battle> GetBattles() => _battles.Values;
        public Battle? GetBattleById(int id) => _battles.TryGetValue(id, out var battle) ? battle : null;

        public void UpdateBattlesFromNotion(IEnumerable<Battle> battles)
        {
            _battles.Clear();
            foreach (var battle in battles)
            {
                _battles[battle.Id] = battle;
            }
        }

        // Characters
        public IEnumerable<Character> GetCharacters() => _characters.Values;
        public Character? GetCharacterById(int id) => _characters.TryGetValue(id, out var character) ? character : null;

        public void UpdateCharactersFromNotion(IEnumerable<Character> characters)
        {
            _characters.Clear();
            foreach (var character in characters)
            {
                _characters[character.Id] = character;
            }
        }

        // Civilizations
        public IEnumerable<Civilization> GetCivilizations() => _civilizations.Values;
        public Civilization? GetCivilizationById(int id) => _civilizations.TryGetValue(id, out var civilization) ? civilization : null;

        public void UpdateCivilizationsFromNotion(IEnumerable<Civilization> civilizations)
        {
            _civilizations.Clear();
            foreach (var civilization in civilizations)
            {
                _civilizations[civilization.Id] = civilization;
            }
        }

        // Territories
        public IEnumerable<Territory> GetTerritories() => _territories.Values;
        public Territory? GetTerritoryById(int id) => _territories.TryGetValue(id, out var territory) ? territory : null;

        public void UpdateTerritoriesFromNotion(IEnumerable<Territory> territories)
        {
            _territories.Clear();
            foreach (var territory in territories)
            {
                _territories[territory.Id] = territory;
            }
        }

        // Relations
        public IEnumerable<CharacterBattle> GetCharacterBattles() => _characterBattles;
        public IEnumerable<CivilizationBattle> GetCivilizationBattles() => _civilizationBattles;
        public IEnumerable<CivilizationAge> GetCivilizationAges() => _civilizationAges;
        public IEnumerable<CivilizationTerritory> GetCivilizationTerritories() => _civilizationTerritories;

        public void UpdateCharacterBattlesFromNotion(IEnumerable<CharacterBattle> relations)
        {
            _characterBattles.Clear();
            foreach (var relation in relations)
            {
                _characterBattles.Add(relation);
            }
        }

        public void UpdateCivilizationBattlesFromNotion(IEnumerable<CivilizationBattle> relations)
        {
            _civilizationBattles.Clear();
            foreach (var relation in relations)
            {
                _civilizationBattles.Add(relation);
            }
        }

        public void UpdateCivilizationAgesFromNotion(IEnumerable<CivilizationAge> relations)
        {
            _civilizationAges.Clear();
            foreach (var relation in relations)
            {
                _civilizationAges.Add(relation);
            }
        }

        public void UpdateCivilizationTerritoriesFromNotion(IEnumerable<CivilizationTerritory> relations)
        {
            _civilizationTerritories.Clear();
            foreach (var relation in relations)
            {
                _civilizationTerritories.Add(relation);
            }
        }
    }
}