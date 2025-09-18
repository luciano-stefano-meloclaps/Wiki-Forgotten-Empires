using Application.Interfaces;
using Application.Models.Dto;
using Application.Models.Request;
using Domain.Interfaces;

namespace Application.Services
{
    public class CivilizationService : ICivilizationService
    {
        private readonly ICivilizationRepository _civilizationRepository;

        public CivilizationService(ICivilizationRepository civilizationRepository)
        {
            _civilizationRepository = civilizationRepository;
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
    }
}