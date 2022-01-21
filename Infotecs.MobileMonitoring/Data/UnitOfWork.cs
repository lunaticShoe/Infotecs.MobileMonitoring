using System;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Repositories;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class UnitOfWork : IUnitOfWork
{
    private readonly IMongoDbContext mongoDbContext;
    private IClientSessionHandle? session;
    private IClientSessionHandle? containerSession;
    private bool disposed;
    
    public UnitOfWork(IMongoDbContext mongoDbContext)
    {
        this.mongoDbContext = mongoDbContext;
        // session = mongoDbContext.Session;
        session = mongoDbContext.StartSession();
    }

    public IEventRepository EventRepository =>
        new EventRepository(mongoDbContext, GetSession());
    public IStatisticsRepository StatisticsRepository => 
        new StatisticsRepository(mongoDbContext, GetSession());
    public IEventDictionaryRepository EventDictionaryRepository => 
        new EventDictionaryRepository(mongoDbContext, GetSession());

    public IMongoDbContext Context => mongoDbContext;
    
    public void CaptureSession(IClientSessionHandle session)
    {
        this.containerSession = session;
    }

    public void ReleaseSession()
    {
        CaptureSession(null);
    }

    private IClientSessionHandle GetSession()
    {
        return containerSession ?? session;
    }
  
    public Task CommitAsync(CancellationToken cancellationToken = default)
    { 
        //return Task.CompletedTask;
        return GetSession().CommitTransactionAsync(cancellationToken);
    }
    
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
