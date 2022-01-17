using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public interface IMongoDbContext
{
    IMongoCollection<StatisticsModel> GetStatisticsCollection();
    IMongoCollection<EventModel> GetEventCollection();
    IMongoDatabase Database { get; }
}
