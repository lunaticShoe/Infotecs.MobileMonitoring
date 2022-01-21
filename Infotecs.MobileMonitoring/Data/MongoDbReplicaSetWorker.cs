using Infotecs.MobileMonitoring.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Data;

public class MongoDbReplicaSetWorker : IMongoDbReplicaSetWorker
{
    private const string AlreadyInitializedError = "AlreadyInitialized";

    private readonly string connectionString;
    private readonly Serilog.ILogger logger;

    public MongoDbReplicaSetWorker(string connectionString, Serilog.ILogger logger)
    {
        this.connectionString = connectionString;
        this.logger = logger;
    }
    
    public void InitReplicaSet()
    {
        var settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
        settings.DirectConnection = true;
        var client = new MongoClient(settings);
        var database = client.GetDatabase("admin");
        
        try
        {
            logger.Information("-> Initializing mongo replica set");
            database.RunCommand<BsonDocument>(BsonDocument.Parse("{ replSetInitiate: 1 }"));
        }
        catch (MongoCommandException ex) when (ex.CodeName == AlreadyInitializedError)
        {
            logger.Debug("Already initialized");
        }
        finally
        {
            logger.Information("<- Initializing mongo replica set");
        }
    }
}
