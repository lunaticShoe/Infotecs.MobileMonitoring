using Infotecs.MobileMonitoring.Models;
using MongoDB.Bson.Serialization;

namespace Infotecs.MobileMonitoring.Data;

public class MongoDbMapper
{
    public static void Map()
    {
        BsonClassMap.RegisterClassMap<StatisticsModel>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(parameter => parameter.Id);
        });
        
        BsonClassMap.RegisterClassMap<EventModel>(cm =>
        {
            cm.MapIdMember(x => x.Id);
            cm.AutoMap();
        });

        BsonClassMap.RegisterClassMap<EventDictionaryModel>(cm =>
        {
            cm.MapIdMember(x => x.Id);
            cm.AutoMap();
        });
    }
}
