namespace Infotecs.MobileMonitoring.Interfaces;

public interface ISessionContainerFactory
{
    ISessionContainer Create(IUnitOfWork unitOfWork);
}
