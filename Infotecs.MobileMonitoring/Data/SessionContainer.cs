using System;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Interfaces;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public sealed class SessionContainer : ISessionContainer
{
    private readonly IUnitOfWork unitOfWork;
    private IClientSessionHandle session;
    private bool disposed;
    
    public SessionContainer(IUnitOfWork unitOfWork)
    {
        this.unitOfWork = unitOfWork;
        session = unitOfWork.Context.StartSession();
        session.StartTransaction();
        unitOfWork.CaptureSession(session);
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
}
