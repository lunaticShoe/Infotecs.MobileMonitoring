using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;

namespace Infotecs.MobileMonitoring.Factories;

public class UnitOfWorkFactory : IUnitOfWorkFactory
{
    private readonly IMongoDbContext mongoDbContext;

    public UnitOfWorkFactory(IMongoDbContext mongoDbContext)
    {
        this.mongoDbContext = mongoDbContext;
    }
    public IUnitOfWork Create()
    {
        return new UnitOfWork(mongoDbContext);
    }
}
