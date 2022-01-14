using Infotecs.MobileMonitoring.Data;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IMongoDbMigration
{
    //int Version { get; }
    
    void Migrate(IMongoDbContext context);
}
