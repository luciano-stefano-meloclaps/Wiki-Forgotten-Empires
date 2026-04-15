using System.Threading;

namespace Application.Interfaces
{
    public interface INotionSyncService
    {
        Task SyncFromNotionAsync(CancellationToken ct);
    }
}
