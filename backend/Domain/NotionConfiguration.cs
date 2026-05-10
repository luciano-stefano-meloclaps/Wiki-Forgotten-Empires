using System;

namespace Domain
{
    public static class NotionConfiguration
    {
        public static bool IsConfigured(Func<string, string?> configurationAccessor)
        {
            var secret = GetSecret(configurationAccessor);
            return !string.IsNullOrWhiteSpace(secret) && HasAnyDatabaseId(configurationAccessor);
        }

        public static string? GetSecret(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:Secret", "Notion__Secret", "NOTION_SECRET", "NOTION__SECRET");
        }

        public static string? GetDatabaseId(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:DatabaseId", "Notion__DatabaseId", "NOTION_DATABASE_ID", "NOTION__DATABASE_ID");
        }

        public static string? GetCharactersDatabaseId(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:CharactersDatabaseId", "Notion__CharactersDatabaseId", "NOTION_CHARACTERS_DATABASE_ID", "NOTION__CHARACTERS_DATABASE_ID");
        }

        public static string? GetCivilizationsDatabaseId(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:CivilizationsDatabaseId", "Notion__CivilizationsDatabaseId", "NOTION_CIVILIZATIONS_DATABASE_ID", "NOTION__CIVILIZATIONS_DATABASE_ID");
        }

        public static string? GetBattlesDatabaseId(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:BattlesDatabaseId", "Notion__BattlesDatabaseId", "NOTION_BATTLES_DATABASE_ID", "NOTION__BATTLES_DATABASE_ID");
        }

        public static string? GetAgesDatabaseId(Func<string, string?> configurationAccessor)
        {
            return GetValue(configurationAccessor, "Notion:AgesDatabaseId", "Notion__AgesDatabaseId", "NOTION_AGES_DATABASE_ID", "NOTION__AGES_DATABASE_ID");
        }

        public static bool HasAnyDatabaseId(Func<string, string?> configurationAccessor)
        {
            return !string.IsNullOrWhiteSpace(GetDatabaseId(configurationAccessor))
                || !string.IsNullOrWhiteSpace(GetCharactersDatabaseId(configurationAccessor))
                || !string.IsNullOrWhiteSpace(GetCivilizationsDatabaseId(configurationAccessor))
                || !string.IsNullOrWhiteSpace(GetBattlesDatabaseId(configurationAccessor))
                || !string.IsNullOrWhiteSpace(GetAgesDatabaseId(configurationAccessor));
        }

        private static string? GetValue(Func<string, string?> configurationAccessor, params string[] keys)
        {
            foreach (var key in keys)
            {
                var value = configurationAccessor(key);
                if (!string.IsNullOrWhiteSpace(value) && !value.Contains("CONFIGURE_IN_APPSETTINGS", StringComparison.OrdinalIgnoreCase))
                {
                    return value;
                }

                var envValue = Environment.GetEnvironmentVariable(key);
                if (!string.IsNullOrWhiteSpace(envValue) && !envValue.Contains("CONFIGURE_IN_APPSETTINGS", StringComparison.OrdinalIgnoreCase))
                {
                    return envValue;
                }
            }

            return null;
        }
    }
}
