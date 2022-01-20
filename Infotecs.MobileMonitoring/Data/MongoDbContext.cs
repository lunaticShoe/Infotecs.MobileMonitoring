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
    public IClientSessionHandle StartSession() => mongoClient.StartSession();
    public IMongoDatabase Database => mongoDatabase;
    public Task<IClientSessionHandle> StartSessionAsync(CancellationToken cancellationToken = default) => 
        mongoClient.StartSessionAsync(null, cancellationToken);

    //  public IMongoClient MongoClient => mongoClient;
    // public async Task<IMongoCollection<StatisticsModel>> PrepareStatisticsCollection(
    //     CancellationToken cancellationToken = default)
    // {
    //
    //     var collection = mongoDatabase.GetCollection<StatisticsModel>(StatisticsCollectionName);
    //     var documentBuilder = Builders<StatisticsModel>.IndexKeys;
    //     var idIndex = new CreateIndexModel<StatisticsModel>(
    //         documentBuilder.Hashed(x => x.Id));
    //
    //     await collection.Indexes.CreateOneAsync(idIndex, null, cancellationToken);
    //     return collection;
    // }
    // public async Task<IMongoCollection<EventModel>> PrepareEventCollection(
    //     CancellationToken cancellationToken = default)
    // {
    //     var collection = mongoDatabase.GetCollection<EventModel>(EventCollectionName);
    //
    //     var documentBuilder = Builders<EventModel>.IndexKeys;
    //     var nameIndex = new CreateIndexModel<EventModel>(
    //         documentBuilder.Ascending(x => x.Name));
    //
    //     var statisticsIdIndex = new CreateIndexModel<EventModel>(
    //         documentBuilder.Hashed(x => x.StatisticsId));
    //
    //     await collection.Indexes.CreateManyAsync(new[]
    //         {
    //             nameIndex, statisticsIdIndex
    //         },
    //         cancellationToken);
    //     return collection;
    // }
}
