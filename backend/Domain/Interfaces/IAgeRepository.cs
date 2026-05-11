using Domain.Entities;

namespace Domain.Interfaces
{
    public interface IAgeRepository
    {
        Task<IEnumerable<Age>> GetAllAges(CancellationToken ct);

        Task<Age?> GetAgeById(int id, CancellationToken ct);

        Task<Age?> GetAgeByName(string name, CancellationToken ct);

        Task<Age> CreateAge(Age age, CancellationToken ct);

        Task UpdateAge(Age age, CancellationToken ct);

        Task DeleteAge(Age age, CancellationToken ct);
    }
}