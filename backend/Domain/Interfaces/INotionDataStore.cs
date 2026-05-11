using Domain.Entities;
using Domain.Relations;

namespace Domain.Interfaces
{
    public interface INotionDataStore
    {
        // Ages
        IEnumerable<Age> GetAges();
        Age? GetAgeById(int id);
        void UpdateAgesFromNotion(IEnumerable<Age> ages);

        // Battles
        IEnumerable<Battle> GetBattles();
        Battle? GetBattleById(int id);
        void UpdateBattlesFromNotion(IEnumerable<Battle> battles);

        // Characters
        IEnumerable<Character> GetCharacters();
        Character? GetCharacterById(int id);
        void UpdateCharactersFromNotion(IEnumerable<Character> characters);

        // Civilizations
        IEnumerable<Civilization> GetCivilizations();
        Civilization? GetCivilizationById(int id);
        void UpdateCivilizationsFromNotion(IEnumerable<Civilization> civilizations);

        // Territories
        IEnumerable<Territory> GetTerritories();
        Territory? GetTerritoryById(int id);
        void UpdateTerritoriesFromNotion(IEnumerable<Territory> territories);

        // Relations
        IEnumerable<CharacterBattle> GetCharacterBattles();
        IEnumerable<CivilizationBattle> GetCivilizationBattles();
        IEnumerable<CivilizationAge> GetCivilizationAges();
        IEnumerable<CivilizationTerritory> GetCivilizationTerritories();

        void UpdateCharacterBattlesFromNotion(IEnumerable<CharacterBattle> relations);
        void UpdateCivilizationBattlesFromNotion(IEnumerable<CivilizationBattle> relations);
        void UpdateCivilizationAgesFromNotion(IEnumerable<CivilizationAge> relations);
        void UpdateCivilizationTerritoriesFromNotion(IEnumerable<CivilizationTerritory> relations);

        // Status
        bool IsInitialized { get; }
        DateTime? LastSyncTime { get; }
        void MarkAsInitialized();
    }
}