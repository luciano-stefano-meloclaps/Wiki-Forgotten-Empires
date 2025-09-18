using Application.Models.Dto;
using Application.Models.Request;

namespace Application.Interfaces
{
    public interface ICivilizationService
    {
        Task<IEnumerable<CivilizationGalleryDto>> GetAllCivilization(CancellationToken ct);

        Task<CivilizationDetailDto?> GetCivizlizationById(int id, CancellationToken ct);

        Task<CivilizationDetailDto> CreateCivilization(CreateCivilizationRequest request, CancellationToken ct);

        Task<bool> UpdateCivilization(int id, UpdateCivilizationRequest request, CancellationToken ct);

        Task<bool> DeleteCivilization(int id, CancellationToken ct);
    }
}