using Infotecs.MobileMonitoring.Interfaces;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class SessionContainer : ISessionContainer
{
    private readonly IUnitOfWork unitOfWork;
    private IClientSessionHandle session;
    private bool disposed;
    
    public SessionContainer(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        session = unitOfWork.Context.StartSession();
        unitOfWork.CaptureSession(session);
    }

    ~SessionContainer()
    {
        Dispose(false);
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
            unitOfWork.ReleaseSession();
            session.Dispose();
        }
        disposed = true;
        
    }
    

    public Task SaveAsync(CancellationToken cancellationToken = default)
    {
        return session.CommitTransactionAsync(cancellationToken);
    }
}
