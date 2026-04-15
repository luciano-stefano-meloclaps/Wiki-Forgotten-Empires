using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Relations;
using ITerritoryRepository = Domain.Interfaces.ITerritoryRepository;

namespace Application.Services
{
    public class CivilizationService : ICivilizationService
    {
        private readonly ICivilizationRepository _civilizationRepository;
        private readonly ITerritoryRepository _territoryRepository;

        public CivilizationService(
            ICivilizationRepository civilizationRepository,
            ITerritoryRepository territoryRepository)
        {
            _civilizationRepository = civilizationRepository;
            _territoryRepository = territoryRepository;
        }

        public async Task<IEnumerable<CivilizationGalleryDto>> GetAllCivilization(CancellationToken ct)
        {
            var civilizations = await _civilizationRepository.GetAllCivilization(ct);
            return civilizations.Select(CivilizationGalleryDto.ToDto);
        }

        public async Task<CivilizationDetailDto?> GetCivizlizationById(int id, CancellationToken ct)
        {
            var civilization = await _civilizationRepository.GetCivizlizationById(id, ct);
            return civilization is null ? null : CivilizationDetailDto.ToDto(civilization);
        }

        public async Task<CivilizationDetailDto> CreateCivilization(CreateCivilizationRequest request, CancellationToken ct)

        {
            var civilization = CreateCivilizationRequest.ToEntity(request);
            await _civilizationRepository.CreateCivilization(civilization, ct);
            await SetTerritoriesAsync(civilization, request.Territories, ct);
            await _civilizationRepository.UpdateCivilization(civilization, ct);
            return CivilizationDetailDto.ToDto(civilization);
        }

        public async Task<bool> UpdateCivilization(int id, UpdateCivilizationRequest request, CancellationToken ct)
        {
            var civilization = await _civilizationRepository.GetCivizlizationById(id, ct);
            if (civilization is null)
            {
                return false;
            }
            UpdateCivilizationRequest.ApplyToEntity(request, civilization);
            if (request.Territories is not null)
            {
                await SetTerritoriesAsync(civilization, request.Territories, ct);
            }
            await _civilizationRepository.UpdateCivilization(civilization, ct);
            return true;
        }

        public async Task<bool> DeleteCivilization(int id, CancellationToken ct)
        {
            var civilization = await _civilizationRepository.GetCivizlizationById(id, ct);
            if (civilization is null)
            {
                return false;
            }

            await _civilizationRepository.DeleteCivilization(civilization, ct);

            return true;
        }

        private async Task SetTerritoriesAsync(Civilization civilization, List<string>? territories, CancellationToken ct)
        {
            if (territories is null)
            {
                return;
            }

            var normalizedNames = territories
                .Where(name => !string.IsNullOrWhiteSpace(name))
                .Select(name => name.Trim())
                .Distinct(StringComparer.OrdinalIgnoreCase)
                .ToList();

            var existingTerritories = await _territoryRepository.GetTerritoriesByNames(normalizedNames, ct);
            var existingMap = existingTerritories
                .ToDictionary(t => t.Name, StringComparer.OrdinalIgnoreCase);

            civilization.Territories.Clear();

            foreach (var territoryName in normalizedNames)
            {
                if (!existingMap.TryGetValue(territoryName, out var territory))
                {
                    territory = await _territoryRepository.CreateTerritory(new Territory { Name = territoryName }, ct);
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
    }
}