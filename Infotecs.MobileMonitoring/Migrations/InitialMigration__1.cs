using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Migrations;

public class InitialMigration__1 : IMongoDbMigration
{
    //public int Version => 1;

    public void Migrate(IMongoDbContext context)
    {
        var documentBuilder = Builders<StatisticsModel>.IndexKeys;
        var userIndex = new CreateIndexModel<StatisticsModel>(
            documentBuilder.Ascending(x => x.UserName));
        
        context.GetStatisticsCollection()
            .Indexes
            .CreateOne(userIndex);
    }
}
