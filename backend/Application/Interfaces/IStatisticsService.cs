using Application.Models.Dto;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IStatisticsService
    {
        Task<GlobalStatsDto> GetGlobalStatsAsync(CancellationToken ct);
    }
}
