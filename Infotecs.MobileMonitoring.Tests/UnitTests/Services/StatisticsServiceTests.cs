using System;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Infotecs.MobileMonitoring.Exceptions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Infotecs.MobileMonitoring.Services;
using Moq;
using Serilog;
using Xunit;

namespace Infotecs.MobileMonitoring.Tests.UnitTests.Services;

public class StatisticsServiceTests
{

    private static readonly Fixture fixture = new();
    
    [Fact]
    public async Task Should_Create_Statistics_Item()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var statisticsRepo = new Mock<IStatisticsRepository>();
        var statisticsService = new StatisticsService(statisticsRepo.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();
        StatisticsModel resultModel = null;

        statisticsRepo
            .Setup(s =>
                s.CreateAsync(It.IsAny<StatisticsModel>(), CancellationToken.None))
            .Callback((StatisticsModel a, CancellationToken _) =>
            {
                resultModel = a;
            });
            
        // Act
        await statisticsService.CreateAsync(statisticsItem);
        // Assert
        resultModel.Should().NotBeNull();
        resultModel.Should().BeEquivalentTo(statisticsItem);
    }
    
    [Fact]
    public async Task Should_NotCreateExistingStatisticsItem_Throws()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var statisticsRepo = new Mock<IStatisticsRepository>();
        var statisticsService = new StatisticsService(statisticsRepo.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();

        statisticsRepo
            .Setup(s => 
                s.GetAsync(statisticsItem.Id, CancellationToken.None))
            .ReturnsAsync(statisticsItem);
        
        // Act
        Func<Task> act = () => statisticsService.CreateAsync(statisticsItem, CancellationToken.None);
        
        // Assert
        await act.Should()
            .ThrowAsync<ElementAlreadyExistsException>("Invalid statistics item created");

    }
    
}
