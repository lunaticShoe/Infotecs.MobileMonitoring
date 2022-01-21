using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Migrations;

public class EventsDictionaryAdded__3 : IMongoDbMigration
{
    public void Migrate(IMongoDbContext context)
    {
        var documentBuilder = Builders<EventDictionaryModel>.IndexKeys;
        var nameIndex = new CreateIndexModel<EventDictionaryModel>(
            documentBuilder.Ascending(x => x.EventName));

        context.GetEventDictionaryCollection().Indexes.CreateMany(new[]
        {
            nameIndex
        });
    }
}
