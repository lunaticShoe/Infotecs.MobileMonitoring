using Infotecs.MobileMonitoring.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class DataContext
{
    private readonly string connectionString;
    private const string StatisticsCollectionName = "statistics";
    private const string DbName = "monitoring";

    public DataContext(string connectionString)
    {
        this.connectionString = connectionString;
        BsonClassMap.RegisterClassMap<StatisticsModel>(cm =>
        {
            cm.AutoMap();
            cm.MapIdMember(parameter => parameter.Id);
        });

        var conventionPack = new ConventionPack();
        conventionPack.Add(new CamelCaseElementNameConvention());
        ConventionRegistry.Register("camelCase", conventionPack, t => true);
    }

    public async Task<IMongoCollection<StatisticsModel>> PrepareStatisticsCollection(
        CancellationToken cancellationToken = default)
    {
        var dbClient = new MongoClient(connectionString);

        var db = dbClient.GetDatabase(DbName);
        var collection = db.GetCollection<StatisticsModel>(StatisticsCollectionName);

        var documentBuilder = Builders<StatisticsModel>.IndexKeys;
        var idIndex = new CreateIndexModel<StatisticsModel>(
            documentBuilder.Hashed(x => x.Id));

        await collection.Indexes.CreateOneAsync(idIndex, null, cancellationToken);
        return collection;
    }
}
