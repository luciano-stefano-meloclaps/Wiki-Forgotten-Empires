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
            if (string.IsNullOrWhiteSpace(notionToken))
            {
                throw new InvalidOperationException("Notion integration is not configured. Configure Notion:Secret in appsettings.");
            }

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", notionToken);
            _httpClient.DefaultRequestHeaders.Remove("Notion-Version");
            _httpClient.DefaultRequestHeaders.Add("Notion-Version", "2022-06-28");

            await SyncDatabaseAsync(_configuration["Notion:AgesDatabaseId"], "age", ct);
            await SyncDatabaseAsync(_configuration["Notion:CivilizationsDatabaseId"], "civilization", ct);
            await SyncDatabaseAsync(_configuration["Notion:BattlesDatabaseId"], "battle", ct);
            await SyncDatabaseAsync(_configuration["Notion:CharactersDatabaseId"], "character", ct);
            
            // Fallback for previous single database logic
            await SyncDatabaseAsync(_configuration["Notion:DatabaseId"], "mixed", ct); 
        }

        private async Task SyncDatabaseAsync(string? databaseId, string defaultType, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(databaseId)) return;

            using var request = new HttpRequestMessage(HttpMethod.Post, $"databases/{databaseId}/query")
            {
                Content = new StringContent("{}", Encoding.UTF8, "application/json")
            };

            var response = await _httpClient.SendAsync(request, ct);
            if (!response.IsSuccessStatusCode) return;

            using var stream = await response.Content.ReadAsStreamAsync(ct);
            var queryResult = await JsonSerializer.DeserializeAsync<NotionQueryResult>(stream, GetJsonSerializerOptions(), ct);
            if (queryResult?.Results == null) return;

            foreach (var page in queryResult.Results)
            {
                await SyncPageAsync(page, defaultType, ct);
            }
        }

        private async Task SyncPageAsync(NotionPage page, string defaultType, CancellationToken ct)
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
                    await SyncCharacterAsync(page, name, ct);
                    break;
                case "battle":
                case "batalla":
                    await SyncBattleAsync(page, name, ct);
                    break;
                case "age":
                case "era":
                case "periodo":
                    await SyncAgeAsync(page, name, ct);
                    break;
                case "civilization":
                case "civilizacion":
                case "civilización":
                default:
                    await SyncCivilizationAsync(page, name, ct);
                    break;
            }
        }

        private async Task SyncAgeAsync(NotionPage page, string name, CancellationToken ct)
        {
            var summary = page.GetText("Summary") ?? page.GetText("Resumen") ?? page.GetText("Description") ?? page.GetText("Descripción");
            var overview = page.GetText("Overview") ?? page.GetText("Visión General") ?? page.GetText("Características") ?? summary;
            var date = page.GetText("Date") ?? page.GetText("Fecha");

            var existingAge = await _ageRepository.GetAgeByName(name.Trim(), ct);
            if (existingAge is null)
            {
                var age = new Age
                {
                    Name = name.Trim(),
                    Summary = summary,
                    Overview = overview,
                    Date = date
                };

                await _ageRepository.CreateAge(age, ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(summary)) existingAge.Summary = summary;
            if (!string.IsNullOrWhiteSpace(overview)) existingAge.Overview = overview;
            if (!string.IsNullOrWhiteSpace(date)) existingAge.Date = date;

            await _ageRepository.UpdateAge(existingAge, ct);
        }

        private async Task SyncCharacterAsync(NotionPage page, string name, CancellationToken ct)
        {
            var roleList = page.GetMultiSelect("Role") ?? page.GetMultiSelect("Rol");
            var roleString = roleList?.FirstOrDefault() ?? page.GetSelect("Role") ?? page.GetSelect("Rol");
            var role = ParseRole(roleString);
            
            var description = page.GetText("Description") ?? page.GetText("Descripción") ?? page.GetText("Descripcion");
            var honorificTitle = page.GetText("HonorificTitle") ?? page.GetText("Título Honorífico") ?? page.GetText("Titulo");
            
            // Age in new Notion is a number/text that acts as life period
            var lifePeriod = page.GetText("LifePeriod") ?? page.GetText("Período de Vida") ?? page.GetText("Periodo") ?? page.GetText("Age");
            var dynasty = page.GetText("Dynasty") ?? page.GetText("Dinastía") ?? page.GetText("Dinastia");
            var imageUrl = page.GetText("ImageUrl") ?? page.GetText("Image") ?? page.GetText("Imagen");

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
                    Role = role
                };
                existing = CharacterCreateRequest.ToEntity(request);
                existing = await _characterRepository.CreateCharacter(existing, ct);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(description)) existing.Description = description;
                if (!string.IsNullOrWhiteSpace(honorificTitle)) existing.HonorificTitle = honorificTitle;
                if (!string.IsNullOrWhiteSpace(lifePeriod)) existing.LifePeriod = lifePeriod;
                if (!string.IsNullOrWhiteSpace(dynasty)) existing.Dynasty = dynasty;
                if (!string.IsNullOrWhiteSpace(imageUrl)) existing.ImageUrl = imageUrl;
                if (role is not null) existing.Role = role;
            }

            var ageRelationIds = page.GetRelationIds("Periodos") ?? page.GetRelationIds("Age") ?? new List<string>();
            if (ageRelationIds.Any())
            {
                var agePage = await FetchNotionPageAsync(ageRelationIds.First(), ct);
                var ageName = agePage.GetTitle("Name") ?? agePage.GetTitle("Title");
                existing.AgeId = await ResolveAgeIdAsync(ageName, ct);
            }

            var civRelationIds = page.GetRelationIds("Civilizations") ?? page.GetRelationIds("Civilizaciones") ?? new List<string>();
            if (civRelationIds.Any())
            {
                var civPage = await FetchNotionPageAsync(civRelationIds.First(), ct);
                var civName = civPage.GetTitle("Name") ?? civPage.GetTitle("Title");
                existing.CivilizationId = await ResolveCivilizationIdAsync(civName, ct);
            }

            var victoryIds = page.GetRelationIds("Victory") ?? page.GetRelationIds("Victorias");
            var defeatIds = page.GetRelationIds("Defeat") ?? page.GetRelationIds("Derrotas");
            
            await SetCharacterBattleRelationsAsync(existing, victoryIds, defeatIds, ct);
            await _characterRepository.UpdateCharacter(existing, ct);
        }

        private async Task SetCharacterBattleRelationsAsync(Character character, List<string>? victoryIds, List<string>? defeatIds, CancellationToken ct)
        {
            var processedBattleIds = new HashSet<int>();

            async Task ProcessRelations(List<string>? notionIds, ParticipantOutcome outcome)
            {
                if (notionIds == null) return;
                foreach (var id in notionIds)
                {
                    var battlePage = await FetchNotionPageAsync(id, ct);
                    var battleName = battlePage.GetTitle("Name") ?? battlePage.GetTitle("Title") ?? battlePage.GetTitle("Conflictos");
                    if (string.IsNullOrWhiteSpace(battleName)) continue;

                    var battle = await _battleRepository.GetBattleByName(battleName, ct);
                    if (battle == null)
                    {
                        battle = await _battleRepository.CreateBattle(new Battle { Name = battleName }, ct);
                    }

                    processedBattleIds.Add(battle.Id);
                    
                    var existingCb = character.Battles.FirstOrDefault(cb => cb.BattleId == battle.Id);
                    if (existingCb == null)
                    {
                        character.Battles.Add(new CharacterBattle
                        {
                            CharacterId = character.Id,
                            BattleId = battle.Id,
                            Outcome = outcome
                        });
                    }
                    else
                    {
                        existingCb.Outcome = outcome;
                    }
                }
            }

            await ProcessRelations(victoryIds, ParticipantOutcome.Victory);
            await ProcessRelations(defeatIds, ParticipantOutcome.Defeat);
        }

        private async Task SyncBattleAsync(NotionPage page, string name, CancellationToken ct)
        {
            var battleDate = page.GetText("Date") ?? page.GetText("Fecha");
            var summary = page.GetText("Summary") ?? page.GetText("Resumen");
            var detailedDescription = page.GetText("DetailedDescription") ?? page.GetText("Description") ?? page.GetText("Descripción");
            var territoryString = page.GetSelect("Territory") ?? page.GetSelect("Territorio");
            var territory = ParseEnum<TerritoryType>(territoryString);

            var existing = await _battleRepository.GetBattleByName(name, ct);
            if (existing is null)
            {
                var request = new BattleCreateRequest
                {
                    Name = name,
                    Date = battleDate,
                    Summary = summary,
                    DetailedDescription = detailedDescription,
                    Territory = territory
                };
                existing = BattleCreateRequest.ToEntity(request);
                existing = await _battleRepository.CreateBattle(existing, ct);
            }
            else
            {
                if (!string.IsNullOrWhiteSpace(battleDate)) existing.Date = battleDate;
                if (!string.IsNullOrWhiteSpace(summary)) existing.Summary = summary;
                if (!string.IsNullOrWhiteSpace(detailedDescription)) existing.DetailedDescription = detailedDescription;
                if (territory is not null) existing.Territory = territory;
            }

            var ageRelationIds = page.GetRelationIds("Periodo") ?? page.GetRelationIds("Age") ?? new List<string>();
            if (ageRelationIds.Any())
            {
                var agePage = await FetchNotionPageAsync(ageRelationIds.First(), ct);
                var ageName = agePage.GetTitle("Name") ?? agePage.GetTitle("Title");
                existing.AgeId = await ResolveAgeIdAsync(ageName, ct);
            }

            await _battleRepository.UpdateBattle(existing, ct);
        }

        private async Task SyncCivilizationAsync(NotionPage page, string name, CancellationToken ct)
        {
            var summary = page.GetText("Summary") ?? page.GetText("Description") ?? page.GetText("Descripción");
            var overview = page.GetText("Overview") ?? page.GetText("Visión General") ?? summary;
            var imageUrl = page.GetText("ImageUrl") ?? page.GetText("Image") ?? page.GetText("Imagen");
            var stateString = page.GetSelect("State") ?? page.GetSelect("Estado");
            var state = ParseEnum<CivilizationState>(stateString);
            var territories = page.GetMultiSelect("Regions") ?? page.GetMultiSelect("Regiones") ?? new List<string>();
            
            var ageRelationIds = page.GetRelationIds("Ages") ?? page.GetRelationIds("Eras") ?? new List<string>();
            var battleRelationIds = page.GetRelationIds("Battles") ?? page.GetRelationIds("Batallas") ?? new List<string>();
            var characterRelationIds = page.GetRelationIds("Characters") ?? page.GetRelationIds("Personajes") ?? new List<string>();

            var existing = await _civilizationRepository.GetCivilizationByName(name.Trim(), ct);
            if (existing is null)
            {
                var civilization = new Civilization
                {
                    Name = name.Trim(),
                    Summary = summary,
                    Overview = overview,
                    ImageUrl = imageUrl,
                    State = state ?? CivilizationState.None
                };

                await _civilizationRepository.CreateCivilization(civilization, ct);
                await SetCivilizationTerritoriesAsync(civilization, territories, ct);
                await SetCivilizationAgeRelationsAsync(civilization, ageRelationIds, ct);
                await SetCivilizationBattleRelationsAsync(civilization, battleRelationIds, ct);
                await SetCivilizationCharacterRelationsAsync(civilization, characterRelationIds, ct);
                await _civilizationRepository.UpdateCivilization(civilization, ct);
                return;
            }

            if (!string.IsNullOrWhiteSpace(summary)) existing.Summary = summary;
            if (!string.IsNullOrWhiteSpace(overview)) existing.Overview = overview;
            if (!string.IsNullOrWhiteSpace(imageUrl)) existing.ImageUrl = imageUrl;
            if (state is not null) existing.State = state ?? CivilizationState.None;

            await SetCivilizationTerritoriesAsync(existing, territories, ct);
            await SetCivilizationAgeRelationsAsync(existing, ageRelationIds, ct);
            await SetCivilizationBattleRelationsAsync(existing, battleRelationIds, ct);
            await SetCivilizationCharacterRelationsAsync(existing, characterRelationIds, ct);
            await _civilizationRepository.UpdateCivilization(existing, ct);
        }

        private async Task SetCivilizationTerritoriesAsync(Civilization civilization, List<string> territoryNames, CancellationToken ct)
        {
            if (!territoryNames.Any()) return;

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
            if (!pageIds.Any()) return;

            foreach (var relationId in pageIds.Distinct())
            {
                var characterPage = await FetchNotionPageAsync(relationId, ct);
                var characterName = characterPage.GetTitle("Name") ?? characterPage.GetTitle("Title") ?? characterPage.GetTitle("Nombre");
                if (string.IsNullOrWhiteSpace(characterName)) continue;

                var existingCharacter = await _characterRepository.GetCharacterByName(characterName, ct);
                if (existingCharacter is null)
                {
                    var roleList = characterPage.GetMultiSelect("Role") ?? characterPage.GetMultiSelect("Rol");
                    var roleString = roleList?.FirstOrDefault() ?? characterPage.GetSelect("Role") ?? characterPage.GetSelect("Rol");
                    var role = ParseRole(roleString);
                    var character = new Character
                    {
                        Name = characterName,
                        Description = characterPage.GetText("Description") ?? characterPage.GetText("Descripción"),
                        HonorificTitle = characterPage.GetText("HonorificTitle") ?? characterPage.GetText("Título Honorífico"),
                        LifePeriod = characterPage.GetText("LifePeriod") ?? characterPage.GetText("Período de Vida") ?? characterPage.GetText("Age"),
                        Dynasty = characterPage.GetText("Dynasty") ?? characterPage.GetText("Dinastía"),
                        ImageUrl = characterPage.GetText("ImageUrl") ?? characterPage.GetText("Image"),
                        CivilizationId = civilization.Id,
                        Role = role
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
            if (!pageIds.Any()) return;

            foreach (var relationId in pageIds.Distinct())
            {
                var agePage = await FetchNotionPageAsync(relationId, ct);
                var ageName = agePage.GetTitle("Name") ?? agePage.GetTitle("Title");
                if (string.IsNullOrWhiteSpace(ageName)) continue;

                var existingAge = await _ageRepository.GetAgeByName(ageName, ct);
                if (existingAge is null)
                {
                    var summary = agePage.GetText("Summary") ?? agePage.GetText("Resumen") ?? agePage.GetText("Descripción");
                    var age = new Age
                    {
                        Name = ageName,
                        Summary = summary,
                        Overview = agePage.GetText("Overview") ?? agePage.GetText("Visión General") ?? summary,
                        Date = agePage.GetText("Date") ?? agePage.GetText("Fecha")
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
            if (!pageIds.Any()) return;

            foreach (var relationId in pageIds.Distinct())
            {
                var battlePage = await FetchNotionPageAsync(relationId, ct);
                var battleName = battlePage.GetTitle("Name") ?? battlePage.GetTitle("Title") ?? battlePage.GetTitle("Conflictos");
                if (string.IsNullOrWhiteSpace(battleName)) continue;

                var existingBattle = await _battleRepository.GetBattleByName(battleName, ct);
                if (existingBattle is null)
                {
                    var territoryString = battlePage.GetSelect("Territory") ?? battlePage.GetSelect("Territorio");
                    var ageRelationIds = battlePage.GetRelationIds("Periodo") ?? battlePage.GetRelationIds("Age") ?? new List<string>();
                    int? ageId = null;
                    if (ageRelationIds.Any()) {
                        var agePage = await FetchNotionPageAsync(ageRelationIds.First(), ct);
                        ageId = await ResolveAgeIdAsync(agePage.GetTitle("Name") ?? agePage.GetTitle("Title"), ct);
                    }

                    var battle = new Battle
                    {
                        Name = battleName,
                        Date = battlePage.GetText("Date") ?? battlePage.GetText("Fecha"),
                        Summary = battlePage.GetText("Summary") ?? battlePage.GetText("Resumen"),
                        DetailedDescription = battlePage.GetText("DetailedDescription") ?? battlePage.GetText("Descripción"),
                        Territory = ParseEnum<TerritoryType>(territoryString),
                        AgeId = ageId
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
            if (string.IsNullOrWhiteSpace(ageName)) return null;

            var normalized = ageName.Trim();
            var existing = await _ageRepository.GetAgeByName(normalized, ct);
            if (existing is not null) return existing.Id;

            var age = new Age { Name = normalized };
            var created = await _ageRepository.CreateAge(age, ct);
            return created.Id;
        }

        private async Task<int?> ResolveCivilizationIdAsync(string? civilizationName, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(civilizationName)) return null;

            var normalized = civilizationName.Trim();
            var existing = await _civilizationRepository.GetCivilizationByName(normalized, ct);
            if (existing is not null) return existing.Id;

            var civilization = new Civilization
            {
                Name = normalized,
                Summary = string.Empty,
                Overview = string.Empty
            };

            var created = await _civilizationRepository.CreateCivilization(civilization, ct);
            return created.Id;
        }

        private static RoleCharacter? ParseRole(string? value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;
            var normalized = value.Trim().ToLowerInvariant();
            return normalized switch
            {
                "rey" or "king" => RoleCharacter.King,
                "cónsul" or "consul" => RoleCharacter.Consul,
                "caballero" or "knight" => RoleCharacter.Knight,
                "emperador" or "emperor" => RoleCharacter.Emperor,
                "faraón" or "faraon" or "pharaoh" => RoleCharacter.Pharaoh,
                "sultán" or "sultan" => RoleCharacter.Sultan,
                "político" or "politico" or "politician" => RoleCharacter.Politician,
                "presidente" or "president" => RoleCharacter.President,
                "líder militar" or "lider militar" or "militaryleader" => RoleCharacter.MilitaryLeader,
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
        }
    }
}
