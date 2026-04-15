using Application.Interfaces;
using Application.Models.Request;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Relations;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace Application.Services
{
    public class NotionSyncService : INotionSyncService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly ICharacterRepository _characterRepository;
        private readonly IBattleRepository _battleRepository;
        private readonly ICivilizationRepository _civilizationRepository;
        private readonly IAgeRepository _ageRepository;
        private readonly ITerritoryRepository _territoryRepository;

        public NotionSyncService(
            HttpClient httpClient,
            IConfiguration configuration,
            ICharacterRepository characterRepository,
            IBattleRepository battleRepository,
            ICivilizationRepository civilizationRepository,
            IAgeRepository ageRepository,
            ITerritoryRepository territoryRepository)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            _characterRepository = characterRepository ?? throw new ArgumentNullException(nameof(characterRepository));
            _battleRepository = battleRepository ?? throw new ArgumentNullException(nameof(battleRepository));
            _civilizationRepository = civilizationRepository ?? throw new ArgumentNullException(nameof(civilizationRepository));
            _ageRepository = ageRepository ?? throw new ArgumentNullException(nameof(ageRepository));
            _territoryRepository = territoryRepository ?? throw new ArgumentNullException(nameof(territoryRepository));
        }

        public async Task SyncFromNotionAsync(CancellationToken ct)
        {
            var notionToken = _configuration["Notion:Secret"];
            var databaseId = _configuration["Notion:DatabaseId"];

            if (string.IsNullOrWhiteSpace(notionToken) || string.IsNullOrWhiteSpace(databaseId))
            {
                throw new InvalidOperationException("Notion integration is not configured. Configure Notion:Secret and Notion:DatabaseId in appsettings.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", notionToken);
            _httpClient.DefaultRequestHeaders.Remove("Notion-Version");
            _httpClient.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

            using var request = new HttpRequestMessage(HttpMethod.Post, $"databases/{databaseId}/query")
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            var queryResult = await JsonSerializer.DeserializeAsync<NotionQueryResult>(stream, GetJsonSerializerOptions(), ct);
            if (queryResult?.Results == null)
            {
                return;
            }

            foreach (var page in queryResult.Results)
            {
                await SyncPageAsync(page, ct);
            }
        }

        private async Task SyncPageAsync(NotionPage page, CancellationToken ct)
        {
            var name = page.GetTitle("Name") ?? page.GetTitle("Title");
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            var entityType = page.GetSelect("EntityType")
                ?? page.GetSelect("Type")
                ?? page.GetSelect("Tipo")
                ?? "Civilization";

            switch (entityType.Trim().ToLowerInvariant())
            {
                case "character":
                    await SyncCharacterAsync(page, name, ct);
                    break;
                case "battle":
                    await SyncBattleAsync(page, name, ct);
                    break;
                case "civilization":
                    await SyncCivilizationAsync(page, name, ct);
                    break;
                default:
                    await SyncCivilizationAsync(page, name, ct);
                    break;
            }
        }

        private async Task SyncCharacterAsync(NotionPage page, string name, CancellationToken ct)
        {
            var role = ParseEnum<RoleCharacter>(page.GetSelect("Role"));
            var ageId = await ResolveAgeIdAsync(page.GetSelect("Age"), ct);
            var civilizationId = await ResolveCivilizationIdAsync(page.GetSelect("Civilization"), ct);
            var description = page.GetText("Description");
            var honorificTitle = page.GetText("HonorificTitle");
            var lifePeriod = page.GetText("LifePeriod");
            var dynasty = page.GetText("Dynasty");
            var imageUrl = page.GetText("ImageUrl");

            var existing = await _characterRepository.GetCharacterByName(name, ct);
            if (existing is null)
            {
                var request = new CharacterCreateRequest
                {
                    Name = name,
                    Description = description,
                    HonorificTitle = honorificTitle,
                    LifePeriod = lifePeriod,
                    Dynasty = dynasty,
                    ImageUrl = imageUrl,
                    Role = role,
                    AgeId = ageId,
                    CivilizationId = civilizationId
                };

                await _characterRepository.CreateCharacter(CharacterCreateRequest.ToEntity(request), ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(description)) existing.Description = description;
            if (!string.IsNullOrWhiteSpace(honorificTitle)) existing.HonorificTitle = honorificTitle;
            if (!string.IsNullOrWhiteSpace(lifePeriod)) existing.LifePeriod = lifePeriod;
            if (!string.IsNullOrWhiteSpace(dynasty)) existing.Dynasty = dynasty;
            if (!string.IsNullOrWhiteSpace(imageUrl)) existing.ImageUrl = imageUrl;
            if (role is not null) existing.Role = role;
            if (ageId.HasValue) existing.AgeId = ageId;
            if (civilizationId.HasValue) existing.CivilizationId = civilizationId;

            await _characterRepository.UpdateCharacter(existing, ct);
        }

        private async Task SyncBattleAsync(NotionPage page, string name, CancellationToken ct)
        {
            var battleDate = page.GetText("Date");
            var summary = page.GetText("Summary");
            var detailedDescription = page.GetText("DetailedDescription") ?? page.GetText("Description");
            var territory = ParseEnum<TerritoryType>(page.GetSelect("Territory"));
            var ageId = await ResolveAgeIdAsync(page.GetSelect("Age"), ct);

            var existing = await _battleRepository.GetBattleByName(name, ct);
            if (existing is null)
            {
                var request = new BattleCreateRequest
                {
                    Name = name,
                    Date = battleDate,
                    Summary = summary,
                    DetailedDescription = detailedDescription,
                    Territory = territory,
                    AgeId = ageId
                };

                await _battleRepository.CreateBattle(BattleCreateRequest.ToEntity(request), ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(battleDate)) existing.Date = battleDate;
            if (!string.IsNullOrWhiteSpace(summary)) existing.Summary = summary;
            if (!string.IsNullOrWhiteSpace(detailedDescription)) existing.DetailedDescription = detailedDescription;
            if (territory is not null) existing.Territory = territory;
            if (ageId.HasValue) existing.AgeId = ageId;

            await _battleRepository.UpdateBattle(existing, ct);
        }

        private async Task SyncCivilizationAsync(NotionPage page, string name, CancellationToken ct)
        {
            var summary = page.GetText("Summary") ?? page.GetText("Description") ?? page.GetText("Overview");
            var overview = page.GetText("Overview") ?? page.GetText("Description") ?? page.GetText("Summary");
            var imageUrl = page.GetText("ImageUrl");
            var state = ParseEnum<CivilizationState>(page.GetSelect("State"))
                ?? ParseEnum<CivilizationState>(page.GetSelect("Estado"));

            var territoryNames = page.GetMultiSelect("Regiones")
                .Concat(page.GetMultiSelect("Regions"))
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var characterPageIds = page.GetRelationIds("Personajes").Concat(page.GetRelationIds("Characters")).ToList();
            var agePageIds = page.GetRelationIds("Periodos").Concat(page.GetRelationIds("Ages")).ToList();
            var battlePageIds = page.GetRelationIds("Batallas").Concat(page.GetRelationIds("Battles")).ToList();

            var existing = await _civilizationRepository.GetCivilizationByName(name, ct);
            Civilization civilization;
            if (existing is null)
            {
                civilization = new Civilization
                {
                    Name = name,
                    Summary = summary,
                    Overview = overview,
                    ImageUrl = imageUrl,
                    State = state ?? CivilizationState.None
                };
                await _civilizationRepository.CreateCivilization(civilization, ct);
            }
            else
            {
                civilization = existing;
                if (!string.IsNullOrWhiteSpace(summary)) civilization.Summary = summary;
                if (!string.IsNullOrWhiteSpace(overview)) civilization.Overview = overview;
                if (!string.IsNullOrWhiteSpace(imageUrl)) civilization.ImageUrl = imageUrl;
                if (state is not null) civilization.State = state.Value;
            }

            await SetCivilizationTerritoriesAsync(civilization, territoryNames, ct);
            await SetCivilizationCharacterRelationsAsync(civilization, characterPageIds, ct);
            await SetCivilizationAgeRelationsAsync(civilization, agePageIds, ct);
            await SetCivilizationBattleRelationsAsync(civilization, battlePageIds, ct);

            await _civilizationRepository.UpdateCivilization(civilization, ct);
        }

        private async Task SetCivilizationTerritoriesAsync(Civilization civilization, List<string> territoryNames, CancellationToken ct)
        {
            if (!territoryNames.Any())
            {
                return;
            }

            var existingTerritories = await _territoryRepository.GetTerritoriesByNames(territoryNames, ct);
            var existingMap = existingTerritories.ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);
            civilization.Territories.Clear();

            foreach (var territoryName in territoryNames)
            {
                if (!existingMap.TryGetValue(territoryName, out var territory))
                {
                    territory = await _territoryRepository.CreateTerritory(new Territory { Name = territoryName }, ct);
                    existingMap[territoryName] = territory;
                }

                civilization.Territories.Add(new CivilizationTerritory
                {
                    Civilization = civilization,
                    CivilizationId = civilization.Id,
                    Territory = territory,
                    TerritoryId = territory.Id
                });
            }
        }

        private async Task SetCivilizationCharacterRelationsAsync(Civilization civilization, List<string> pageIds, CancellationToken ct)
        {
            foreach (var relationId in pageIds.Distinct())
            {
                var characterPage = await FetchNotionPageAsync(relationId, ct);
                var characterName = characterPage.GetTitle("Name") ?? characterPage.GetTitle("Title");
                if (string.IsNullOrWhiteSpace(characterName))
                {
                    continue;
                }

                var existingCharacter = await _characterRepository.GetCharacterByName(characterName, ct);
                if (existingCharacter is null)
                {
                    var character = new Character
                    {
                        Name = characterName,
                        Description = characterPage.GetText("Description"),
                        HonorificTitle = characterPage.GetText("HonorificTitle"),
                        LifePeriod = characterPage.GetText("LifePeriod"),
                        Dynasty = characterPage.GetText("Dynasty"),
                        ImageUrl = characterPage.GetText("ImageUrl"),
                        CivilizationId = civilization.Id,
                        Role = ParseEnum<RoleCharacter>(characterPage.GetSelect("Role"))
                    };

                    await _characterRepository.CreateCharacter(character, ct);
                    continue;
                }

                existingCharacter.CivilizationId = civilization.Id;
                await _characterRepository.UpdateCharacter(existingCharacter, ct);
            }
        }

        private async Task SetCivilizationAgeRelationsAsync(Civilization civilization, List<string> pageIds, CancellationToken ct)
        {
            foreach (var relationId in pageIds.Distinct())
            {
                var agePage = await FetchNotionPageAsync(relationId, ct);
                var ageName = agePage.GetTitle("Name") ?? agePage.GetTitle("Title");
                if (string.IsNullOrWhiteSpace(ageName))
                {
                    continue;
                }

                var existingAge = await _ageRepository.GetAgeByName(ageName, ct);
                if (existingAge is null)
                {
                    var age = new Age
                    {
                        Name = ageName,
                        Summary = agePage.GetText("Summary") ?? agePage.GetText("Description"),
                        Overview = agePage.GetText("Overview"),
                        Date = agePage.GetText("Date")
                    };

                    existingAge = await _ageRepository.CreateAge(age, ct);
                }

                if (!civilization.Ages.Any(ca => ca.AgeId == existingAge.Id))
                {
                    civilization.Ages.Add(new CivilizationAge
                    {
                        Civilization = civilization,
                        CivilizationId = civilization.Id,
                        Age = existingAge,
                        AgeId = existingAge.Id
                    });
                }
            }
        }

        private async Task SetCivilizationBattleRelationsAsync(Civilization civilization, List<string> pageIds, CancellationToken ct)
        {
            foreach (var relationId in pageIds.Distinct())
            {
                var battlePage = await FetchNotionPageAsync(relationId, ct);
                var battleName = battlePage.GetTitle("Name") ?? battlePage.GetTitle("Title");
                if (string.IsNullOrWhiteSpace(battleName))
                {
                    continue;
                }

                var existingBattle = await _battleRepository.GetBattleByName(battleName, ct);
                if (existingBattle is null)
                {
                    var battle = new Battle
                    {
                        Name = battleName,
                        Date = battlePage.GetText("Date"),
                        Summary = battlePage.GetText("Summary"),
                        DetailedDescription = battlePage.GetText("DetailedDescription") ?? battlePage.GetText("Description"),
                        Territory = ParseEnum<TerritoryType>(battlePage.GetSelect("Territory")),
                        AgeId = await ResolveAgeIdAsync(battlePage.GetSelect("Age"), ct)
                    };

                    existingBattle = await _battleRepository.CreateBattle(battle, ct);
                }

                if (!civilization.Battles.Any(cb => cb.BattleId == existingBattle.Id))
                {
                    civilization.Battles.Add(new CivilizationBattle
                    {
                        Civilization = civilization,
                        CivilizationId = civilization.Id,
                        Battle = existingBattle,
                        BattleId = existingBattle.Id
                    });
                }
            }
        }

        private async Task<NotionPage> FetchNotionPageAsync(string pageId, CancellationToken ct)
        {
            var response = await _httpClient.GetAsync($"pages/{pageId}", ct);
            response.EnsureSuccessStatusCode();
            using var stream = await response.Content.ReadAsStreamAsync(ct);
            var notionPage = await JsonSerializer.DeserializeAsync<NotionPage>(stream, GetJsonSerializerOptions(), ct);
            return notionPage ?? throw new InvalidOperationException($"No se pudo leer la página de Notion {pageId}.");
        }

        private async Task<int?> ResolveAgeIdAsync(string? ageName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(ageName))
            {
                return null;
            }

            var normalized = ageName.Trim();
            var existing = await _ageRepository.GetAgeByName(normalized, ct);
            if (existing is not null)
            {
                return existing.Id;
            }

            var age = new Age
            {
                Name = normalized
            };
            var created = await _ageRepository.CreateAge(age, ct);
            return created.Id;
        }

        private async Task<int?> ResolveCivilizationIdAsync(string? civilizationName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(civilizationName))
            {
                return null;
            }

            var normalized = civilizationName.Trim();
            var existing = await _civilizationRepository.GetCivilizationByName(normalized, ct);
            if (existing is not null)
            {
                return existing.Id;
            }

            var civilization = new Civilization
            {
                Name = normalized,
                Summary = string.Empty,
                Overview = string.Empty
            };

            var created = await _civilizationRepository.CreateCivilization(civilization, ct);
            return created.Id;
        }

        private static TEnum? ParseEnum<TEnum>(string? value)
            where TEnum : struct, Enum
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

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
                if (!Properties.TryGetValue(propertyName, out var property))
                {
                    return null;
                }

                if (property.TryGetProperty("title", out var titleElement))
                {
                    return GetPlainTextFromRichText(titleElement);
                }

                return null;
            }

            public string? GetText(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property))
                {
                    return null;
                }

                if (property.TryGetProperty("rich_text", out var richTextElement))
                {
                    return GetPlainTextFromRichText(richTextElement);
                }

                if (property.TryGetProperty("title", out var titleElement))
                {
                    return GetPlainTextFromRichText(titleElement);
                }

                if (property.TryGetProperty("select", out var selectElement) && selectElement.TryGetProperty("name", out var nameElement))
                {
                    return nameElement.GetString();
                }

                return null;
            }

            public string? GetSelect(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property))
                {
                    return null;
                }

                if (property.TryGetProperty("select", out var selectElement) && selectElement.TryGetProperty("name", out var nameElement))
                {
                    return nameElement.GetString();
                }

                return null;
            }

            public List<string> GetMultiSelect(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property))
                {
                    return new List<string>();
                }

                if (!property.TryGetProperty("multi_select", out var multiSelectElement) || multiSelectElement.ValueKind != JsonValueKind.Array)
                {
                    return new List<string>();
                }

                var results = new List<string>();
                foreach (var item in multiSelectElement.EnumerateArray())
                {
                    if (item.TryGetProperty("name", out var nameElement) && nameElement.ValueKind == JsonValueKind.String)
                    {
                        var name = nameElement.GetString();
                        if (!string.IsNullOrWhiteSpace(name))
                        {
                            results.Add(name.Trim());
                        }
                    }
                }

                return results;
            }

            public List<string> GetRelationIds(string propertyName)
            {
                if (!Properties.TryGetValue(propertyName, out var property))
                {
                    return new List<string>();
                }

                if (!property.TryGetProperty("relation", out var relationElement) || relationElement.ValueKind != JsonValueKind.Array)
                {
                    return new List<string>();
                }

                var results = new List<string>();
                foreach (var item in relationElement.EnumerateArray())
                {
                    if (item.TryGetProperty("id", out var idElement) && idElement.ValueKind == JsonValueKind.String)
                    {
                        var id = idElement.GetString();
                        if (!string.IsNullOrWhiteSpace(id))
                        {
                            results.Add(id.Trim());
                        }
                    }
                }

                return results;
            }

            private static string? GetPlainTextFromRichText(JsonElement element)
            {
                if (element.ValueKind != JsonValueKind.Array)
                {
                    return null;
                }

                foreach (var item in element.EnumerateArray())
                {
                    if (item.TryGetProperty("plain_text", out var plainText) && plainText.ValueKind == JsonValueKind.String)
                    {
                        var text = plainText.GetString();
                        if (!string.IsNullOrWhiteSpace(text))
                        {
                            return text.Trim();
                        }
                    }
                }

                return null;
            }
        }
    }
}
