using System;
using System.Linq;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Infotecs.MobileMonitoring.Models;
using Infotecs.MobileMonitoring.Repositories;
using Infotecs.MobileMonitoring.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Infotecs.MobileMonitoring.Tests.IntegrationTests.Repositories;

public class EventRepositoryTests : IClassFixture<MongoDbFixture>
{
    private static readonly Fixture fixture = new();
    private readonly MongoDbFixture mongoDbFixture;

    public EventRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        this.mongoDbFixture = mongoDbFixture;
    }

    [Fact]
    public async Task Should_Create_Events_List()
    {
        // Arrange
        var statId = Guid.NewGuid();
        var repo = new EventRepository(mongoDbFixture.Context);
        var events = fixture.Build<EventModel>()
            .With(e => e.StatisticsId, statId)
            .CreateMany()
            .ToArray();
        // Act
        await repo.CreateRangeAsync(events);
        // Assert
        var resultEvents = await repo.GetListAsync(statId);
        resultEvents.Should().NotBeNullOrEmpty();
    }
}
