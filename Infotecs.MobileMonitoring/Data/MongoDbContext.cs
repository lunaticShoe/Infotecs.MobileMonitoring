using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class MongoDbContext : IMongoDbContext
{
    private const string StatisticsCollectionName = "statistics";
    private const string EventCollectionName = "events";
    private const string EventDictionaryCollectionName = "eventDictionary";
    private const string DbName = "monitoring";

    private readonly MongoClient mongoClient;
    private readonly IMongoDatabase mongoDatabase;
    //private readonly IClientSessionHandle session;
    
    public MongoDbContext(string connectionString)
    {
        mongoClient = new MongoClient(connectionString);
        mongoDatabase = mongoClient.GetDatabase(DbName);
        
    }


    public IMongoCollection<StatisticsModel> GetStatisticsCollection() => 
        mongoDatabase.GetCollection<StatisticsModel>(StatisticsCollectionName);
        
    public IMongoCollection<EventModel> GetEventCollection() => 
        mongoDatabase.GetCollection<EventModel>(EventCollectionName);
    public IMongoCollection<EventDictionaryModel> GetEventDictionaryCollection() => 
        mongoDatabase.GetCollection<EventDictionaryModel>(EventDictionaryCollectionName);
    public IClientSessionHandle StartSession() => mongoClient.StartSession();
    public IMongoDatabase Database => mongoDatabase;
    public Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default) => 
        mongoClient.StartSessionAsync(null, cancellationToken);
}
