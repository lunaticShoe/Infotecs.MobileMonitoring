using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Repositories;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoDbContext mongoDbContext;
    private IEventRepository? eventRepository;
    private IStatisticsRepository? statisticsRepository;
    private IClientSessionHandle? session;
    private bool disposed;
    
    public UnitOfWork(IMongoDbContext mongoDbContext)
    {
        this.mongoDbContext = mongoDbContext;
        // session = mongoDbContext.Session;
        session = mongoDbContext.StartSession();
        eventRepository = new EventRepository(mongoDbContext, session);
        statisticsRepository = new StatisticsRepository(mongoDbContext, session);
    }

    // ~UnitOfWork()
    // {
    //     Dispose(false);
    // }

    public IEventRepository EventRepository =>
        eventRepository; //??= new EventRepository(mongoDbContext);

    public IStatisticsRepository StatisticsRepository =>
        statisticsRepository; //??= new StatisticsRepository(mongoDbContext);

    public IMongoDbContext Context => mongoDbContext;

    private object thisLock = new();
    
    
    
    public void CaptureSession(IClientSessionHandle? session)
    {
        lock (thisLock)
        {
            eventRepository = new EventRepository(mongoDbContext, session);
            statisticsRepository = new StatisticsRepository(mongoDbContext, session);
        }
    }

    public void ReleaseSession()
    {
        CaptureSession(session);
    }
    //
    // public async Task ExecuteTransactionAsync(Func<IUnitOfWork, Task> transaction, CancellationToken cancellationToken = default)
    // {
    //     try
    //     {
    //         using (session = await mongoDbContext.StartSessionAsync(cancellationToken))
    //         {
    //             RecreateRepositories(session);
    //             await transaction(this);
    //         }
    //     }
    //     finally
    //     {
    //         RecreateRepositories(null);
    //     }
    // }
    //
    // public Task SaveAsync()
    // {
    //     if (session is null) return Task.CompletedTask;
    //     return session.CommitTransactionAsync();
    // }
    //
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
    
    private void Dispose(bool disposing)
    {
        if (disposed) return;
        if (disposing)
        {
            session.Dispose();
        }
    
        disposed = true;
    }
}
