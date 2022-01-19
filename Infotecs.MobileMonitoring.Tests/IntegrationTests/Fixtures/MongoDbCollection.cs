using Xunit;

namespace Infotecs.MobileMonitoring.Tests.IntegrationTests.Fixtures;
[CollectionDefinition("Mongo collection")]
public class MongoDbCollection : ICollectionFixture<MongoDbFixture>
{
    
}
