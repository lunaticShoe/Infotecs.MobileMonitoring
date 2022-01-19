using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using Docker.DotNet;
using Docker.DotNet.Models;
using FluentAssertions;
using FluentAssertions.Equivalency;
using Infotecs.MobileMonitoring.Models;
using Infotecs.MobileMonitoring.Repositories;
using Infotecs.MobileMonitoring.Tests.IntegrationTests.Fixtures;
using Xunit;

namespace Infotecs.MobileMonitoring.Tests.IntegrationTests.Repositories;
[CollectionDefinition("Mongo collection")]
public class StatisticsRepositoryTests : IClassFixture<MongoDbFixture>
{
    private static readonly Fixture fixture = new();
    private readonly MongoDbFixture mongoDbFixture;

    public StatisticsRepositoryTests(MongoDbFixture mongoDbFixture)
    {
        this.mongoDbFixture = mongoDbFixture;
    }
    
    [Fact]
    public async Task Should_Create_Statistics_Item()
    {
        // Arrange
        var repo = new StatisticsRepository(mongoDbFixture.Context);
        var statisticsModel = fixture.Create<StatisticsModel>();
        // Act
        await repo.CreateAsync(statisticsModel);
        // Assert
        var resultModel = await repo.GetAsync(statisticsModel.Id);
        resultModel.Should().NotBeNull();
        resultModel.Should().BeEquivalentTo(statisticsModel,
            options =>
            {
                options.Using<StatisticsModel>(ctx => 
                    ctx.Subject.CreatedAt.Date.Should().Be(ctx.Expectation.CreatedAt.Date));
                options.Excluding(s => s.CreatedAt);
                return options;
            });
    }
    [Fact]
    public async Task Should_UpdateStatistics_StatisticsModel()
    {
        // Arrange
        var repo = new StatisticsRepository(mongoDbFixture.Context);
        var statisticsModel = fixture.Create<StatisticsModel>();
        var userName = "statistics-unit-test";
        await repo.CreateAsync(statisticsModel);
        statisticsModel.UserName = userName;
        // Act
        await repo.UpdateAsync(statisticsModel);
        // Assert
        var resultModel = await repo.GetAsync(statisticsModel.Id);
        resultModel.Should().NotBeNull();
        resultModel.UserName.Should().Be(userName);
    }

}
