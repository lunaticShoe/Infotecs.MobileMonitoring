using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Migrations;

public class EventsAddedMigration__2 : IMongoDbMigration
{
    //public int Version => 2;
    public void Migrate(IMongoDbContext context)
    {
        var documentBuilder = Builders<EventModel>.IndexKeys;
        var nameIndex = new CreateIndexModel<EventModel>(
            documentBuilder.Ascending(x => x.Name));

        var statisticsIdIndex = new CreateIndexModel<EventModel>(
            documentBuilder.Hashed(x => x.StatisticsId));

        context.GetEventCollection().Indexes.CreateMany(new[]
        {
            nameIndex, statisticsIdIndex
        });
    }
}
