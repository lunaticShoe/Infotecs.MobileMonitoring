using Infotecs.MobileMonitoring.Data;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IEventRepository EventRepository { get; }
    IStatisticsRepository StatisticsRepository { get; }
    IMongoDbContext Context { get; }

    // Task ExecuteTransactionAsync(Func<IUnitOfWork, Task> transaction, CancellationToken cancellationToken = default);
    //
    // Task SaveAsync();
    void CaptureSession(IClientSessionHandle? session);
    void ReleaseSession();
}
