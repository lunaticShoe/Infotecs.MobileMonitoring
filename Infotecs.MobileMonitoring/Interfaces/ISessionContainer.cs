namespace Infotecs.MobileMonitoring.Interfaces;

public interface ISessionContainer : IDisposable
{
    Task SaveAsync(CancellationToken cancellationToken = default);
}
