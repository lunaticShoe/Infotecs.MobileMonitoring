using Infotecs.MobileMonitoring.Interfaces;

namespace Infotecs.MobileMonitoring.Data;

public class SessionContainerFactory : ISessionContainerFactory
{
    public ISessionContainer Create(IUnitOfWork unitOfWork)
    {
        return new SessionContainer(unitOfWork);
    }
}
