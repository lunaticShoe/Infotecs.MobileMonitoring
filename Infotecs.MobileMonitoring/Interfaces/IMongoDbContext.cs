using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public interface IMongoDbContext
{
    IMongoCollection<StatisticsModel> GetStatisticsCollection();
    IMongoCollection<EventModel> GetEventCollection();
    IMongoDatabase Database { get; }
   // IClientSessionHandle Session { get; }
    IClientSessionHandle StartSession();
    Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default);
    IMongoCollection<EventDictionaryModel> GetEventDictionaryCollection();
}
