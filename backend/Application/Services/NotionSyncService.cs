using Application.Interfaces;
using Domain.Interfaces;
using Application.Models.Request;
using Domain;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Relations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Application.Services
{
    public class NotionSyncService : INotionSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly INotionDataStore _notionStore;
        private readonly ILogger<NotionSyncService> _logger;

        public NotionSyncService(
            HttpClient httpClient,
            IConfiguration configuration,
            INotionDataStore notionStore,
            ILogger<NotionSyncService> logger)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _notionStore = notionStore ?? throw new ArgumentNullException(nameof(notionStore));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task SyncFromNotionAsync(CancellationToken ct)
        {
            var notionToken = NotionConfiguration.GetSecret(key => _configuration[key]);
            if (string.IsNullOrWhiteSpace(notionToken))
            {
                throw new InvalidOperationException("Notion integration is not configured. Configure Notion:Secret in appsettings.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", notionToken);
            _httpClient.DefaultRequestHeaders.Remove("Notion-Version");
            _httpClient.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

            var ages = new List<Age>();
            var civilizations = new List<Civilization>();
            var battles = new List<Battle>();
            var characters = new List<Character>();
            var territories = new List<Territory>();

            await SyncDatabaseAsync(NotionConfiguration.GetAgesDatabaseId(key => _configuration[key]), "age", ages, civilizations, battles, characters, territories, ct);
            await SyncDatabaseAsync(NotionConfiguration.GetCivilizationsDatabaseId(key => _configuration[key]), "civilization", ages, civilizations, battles, characters, territories, ct);
            await SyncDatabaseAsync(NotionConfiguration.GetBattlesDatabaseId(key => _configuration[key]), "battle", ages, civilizations, battles, characters, territories, ct);
            await SyncDatabaseAsync(NotionConfiguration.GetCharactersDatabaseId(key => _configuration[key]), "character", ages, civilizations, battles, characters, territories, ct);

            // Fallback for previous single database logic
            await SyncDatabaseAsync(NotionConfiguration.GetDatabaseId(key => _configuration[key]), "mixed", ages, civilizations, battles, characters, territories, ct);

            // Update the data store
            _notionStore.UpdateAgesFromNotion(ages);
            _notionStore.UpdateCivilizationsFromNotion(civilizations);
            _notionStore.UpdateBattlesFromNotion(battles);
            _notionStore.UpdateCharactersFromNotion(characters);
            _notionStore.UpdateTerritoriesFromNotion(territories);

            // Mark as initialized after first successful sync
            _notionStore.MarkAsInitialized();

            _logger.LogInformation("Notion sync completed. Ages: {AgeCount}, Civilizations: {CivCount}, Battles: {BattleCount}, Characters: {CharCount}, Territories: {TerrCount}",
                ages.Count, civilizations.Count, battles.Count, characters.Count, territories.Count);
        }

        private async Task SyncDatabaseAsync(string? databaseId, string defaultType,
            List<Age> ages, List<Civilization> civilizations, List<Battle> battles, List<Character> characters, List<Territory> territories,
            CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(databaseId)) return;

            using var request = new HttpRequestMessage(HttpMethod.Post, $"databases/{databaseId}/query")
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("Failed to query Notion database {DatabaseId}: {StatusCode}", databaseId, response.StatusCode);
                return;
            }

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            var queryResult = await JsonSerializer.DeserializeAsync<NotionQueryResult>(stream, GetJsonSerializerOptions(), ct);
            if (queryResult?.Results == null) return;

            foreach (var page in queryResult.Results)
            {
                await SyncPageAsync(page, defaultType, ages, civilizations, battles, characters, territories, ct);
            }
        }

        private async Task SyncPageAsync(NotionPage page, string defaultType,
            List<Age> ages, List<Civilization> civilizations, List<Battle> battles, List<Character> characters, List<Territory> territories,
            CancellationToken ct)
        {
            var name = page.GetTitle("Name") ?? page.GetTitle("Title") ?? page.GetTitle("Conflictos") ?? page.GetTitle("Nombre");
            if (string.IsNullOrWhiteSpace(name)) return;

            var entityType = page.GetSelect("EntityType")
                ?? page.GetSelect("Type")
                ?? page.GetSelect("Tipo")
                ?? defaultType;

            switch (entityType.Trim().ToLowerInvariant())
            {
                case "character":
                case "personaje":
                    var character = await CreateCharacterFromPageAsync(page, name, ct);
                    if (character != null) characters.Add(character);
                    break;
                case "battle":
                case "batalla":
                    var battle = await CreateBattleFromPageAsync(page, name, ct);
                    if (battle != null) battles.Add(battle);
                    break;
                case "age":
                case "era":
                case "periodo":
                    var age = CreateAgeFromPage(page, name);
                    ages.Add(age);
                    break;
                case "civilization":
                case "civilizacion":
                case "civilización":
                default:
                    var civilization = await CreateCivilizationFromPageAsync(page, name, ct);
                    if (civilization != null) civilizations.Add(civilization);
                    break;
            }
        }

        private Age CreateAgeFromPage(NotionPage page, string name)
        {
            var summary = page.GetText("Summary") ?? page.GetText("Resumen") ?? page.GetText("Description") ?? page.GetText("Descripción");
            var overview = page.GetText("Overview") ?? page.GetText("Visión General") ?? page.GetText("Características") ?? summary;
            var date = page.GetText("Date") ?? page.GetText("Fecha");

            return new Age
            {
                Id = GenerateId(), // Need to implement ID generation
                Name = name.Trim(),
                Summary = summary,
                Overview = overview,
                Date = date
            };
        }

        private async Task<Character?> CreateCharacterFromPageAsync(NotionPage page, string name, CancellationToken ct)
        {
            var roleList = page.GetMultiSelect("Role") ?? page.GetMultiSelect("Rol");
            var roleString = roleList?.FirstOrDefault() ?? page.GetSelect("Role") ?? page.GetSelect("Rol");
            var role = ParseRole(roleString);

            var description = page.GetText("Description") ?? page.GetText("Descripción") ?? page.GetText("Descripcion");
            var honorificTitle = page.GetText("HonorificTitle") ?? page.GetText("Título Honorífico") ?? page.GetText("Titulo");

            var lifePeriod = page.GetText("LifePeriod") ?? page.GetText("Período de Vida") ?? page.GetText("Periodo") ?? page.GetText("Age");
            var dynasty = page.GetText("Dynasty") ?? page.GetText("Dinastía") ?? page.GetText("Dinastia");
            var imageUrl = page.GetText("ImageUrl") ?? page.GetText("Image") ?? page.GetText("Imagen");

            var request = new CharacterCreateRequest
            {
                Name = name,
                Description = description,
                HonorificTitle = honorificTitle,
                LifePeriod = lifePeriod,
                Dynasty = dynasty,
                ImageUrl = imageUrl,
                Role = role
            };

            var character = CharacterCreateRequest.ToEntity(request);
            character.Id = GenerateId();

            // TODO: Resolve relations in a second pass
            // For now, create basic entity

            return character;
        }

        private async Task<Battle?> CreateBattleFromPageAsync(NotionPage page, string name, CancellationToken ct)
        {
            var battleDate = page.GetText("Date") ?? page.GetText("Fecha");
            var summary = page.GetText("Summary") ?? page.GetText("Resumen");
            var detailedDescription = page.GetText("DetailedDescription") ?? page.GetText("Description") ?? page.GetText("Descripción");

            var request = new BattleCreateRequest
            {
                Name = name,
                Date = battleDate,
                Summary = summary,
                DetailedDescription = detailedDescription
            };

            var battle = BattleCreateRequest.ToEntity(request);
            battle.Id = GenerateId();

            return battle;
        }

        private async Task<Civilization?> CreateCivilizationFromPageAsync(NotionPage page, string name, CancellationToken ct)
        {
            var summary = page.GetText("Summary") ?? page.GetText("Description") ?? page.GetText("Descripción");
            var overview = page.GetText("Overview") ?? page.GetText("Visión General") ?? summary;
            var imageUrl = page.GetText("ImageUrl") ?? page.GetText("Image") ?? page.GetText("Imagen");

            var civilization = new Civilization
            {
                Id = GenerateId(),
                Name = name.Trim(),
                Summary = summary,
                Overview = overview,
                ImageUrl = imageUrl,
                State = CivilizationState.None // Default
            };

            return civilization;
        }

        private static RoleCharacter? ParseRole(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var normalized = value.Trim().ToLowerInvariant();
            return normalized switch
            {
                "rey" or "king" => RoleCharacter.King,
                "consul" or "consul" => RoleCharacter.Consul,
                "caballero" or "knight" => RoleCharacter.Knight,
                "emperador" or "emperor" => RoleCharacter.Emperor,
                "faraon" or "faraon" or "pharaoh" => RoleCharacter.Pharaoh,
                "sultan" or "sultan" => RoleCharacter.Sultan,
                "politico" or "politico" or "politician" => RoleCharacter.Politician,
                "presidente" or "president" => RoleCharacter.President,
                "lider militar" or "lider militar" or "militaryleader" => RoleCharacter.MilitaryLeader,
                _ => ParseEnum<RoleCharacter>(value)
            };
        }

        private static TEnum? ParseEnum<TEnum>(string? value) where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            return Enum.TryParse(value.Trim(), true, out TEnum result) ? result : null;
        }

        private static JsonSerializerOptions GetJsonSerializerOptions()
        {
            return new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
            };
        }

        private static int _idCounter = 1;
        private static int GenerateId() => Interlocked.Increment(ref _idCounter);

        private sealed class NotionQueryResult
        {
            public List<NotionPage>? Results { get; set; }
        }

        private sealed class NotionPage
        {
            public string? Id { get; set; }
            public Dictionary<string, JsonElement> Properties { get; set; } = new();

            public string? GetTitle(string propertyName)
            {
                if (TryGetProperty(propertyName, out var property) && property.TryGetProperty("title", out var titleElement))
                {
                    return GetPlainTextFromRichText(titleElement);
                }

                foreach (var candidate in Properties.Values)
                {
                    if (candidate.TryGetProperty("title", out var candidateTitleElement))
                    {
                        var title = GetPlainTextFromRichText(candidateTitleElement);
                        if (!string.IsNullOrWhiteSpace(title)) return title;
                    }
                }
                return null;
            }

            public string? GetText(string propertyName)
            {
                if (!TryGetProperty(propertyName, out var property)) return null;

                if (property.TryGetProperty("rich_text", out var richTextElement)) return GetPlainTextFromRichText(richTextElement);
                if (property.TryGetProperty("title", out var titleElement)) return GetPlainTextFromRichText(titleElement);
                if (property.TryGetProperty("select", out var selectElement) && selectElement.TryGetProperty("name", out var nameElement)) return nameElement.GetString();
                if (property.TryGetProperty("number", out var numberElement) && numberElement.ValueKind == JsonValueKind.Number) return numberElement.GetRawText();

                return null;
            }

            public string? GetSelect(string propertyName)
            {
                if (!TryGetProperty(propertyName, out var property)) return null;
                if (property.TryGetProperty("select", out var selectElement) && selectElement.TryGetProperty("name", out var nameElement)) return nameElement.GetString();
                return null;
            }

            private bool TryGetProperty(string propertyName, out JsonElement property)
            {
                if (Properties.TryGetValue(propertyName, out property)) return true;

                foreach (var kv in Properties)
                {
                    if (string.Equals(kv.Key, propertyName, StringComparison.OrdinalIgnoreCase))
                    {
                        property = kv.Value;
                        return true;
                    }
                }
                property = default;
                return false;
            }

            public List<string>? GetMultiSelect(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property)) return null;
                if (!property.TryGetProperty("multi_select", out var multiSelectElement) || multiSelectElement.ValueKind != JsonValueKind.Array) return null;

                var results = new List<string>();
                foreach (var item in multiSelectElement.EnumerateArray())
                {
                    if (item.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
                    {
                        var name = nameElement.GetString();
                        if (!string.IsNullOrWhiteSpace(name)) results.Add(name.Trim());
                    }
                }
                return results;
            }

            public List<string>? GetRelationIds(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property)) return null;
                if (!property.TryGetProperty("relation", out var relationElement) || relationElement.ValueKind != JsonValueKind.Array) return null;

                var results = new List<string>();
                foreach (var item in relationElement.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idElement) && idElement.ValueKind == JsonValueKind.String)
                    {
                        var id = idElement.GetString();
                        if (!string.IsNullOrWhiteSpace(id)) results.Add(id.Trim());
                    }
                }
                return results;
            }

            private static string? GetPlainTextFromRichText(JsonElement element)
            {
                if (element.ValueKind != JsonValueKind.Array) return null;

                foreach (var item in element.EnumerateArray())
                {
                    if (item.TryGetProperty("plain_text", out var plainText) && plainText.ValueKind == JsonValueKind.String)
                    {
                        var text = plainText.GetString();
                        if (!string.IsNullOrWhiteSpace(text)) return text.Trim();
                    }
                }
                return null;
            }

            private static int _idCounter = 1;
            private static int GenerateId() => Interlocked.Increment(ref _idCounter);
        }
    }
}
