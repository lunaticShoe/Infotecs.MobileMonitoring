using System;
using System.Collections.Generic;
using System.Linq;
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

public class EventServiceTests
{
    private static readonly Fixture fixture = new();
    [Fact]
    public async Task Should_Create_Events_Range()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var statisticsRepo = new Mock<IStatisticsRepository>();
        var eventsRepo = new Mock<IEventRepository>();
        var eventService = new EventService(eventsRepo.Object, statisticsRepo.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();
        var events = fixture
            .Build<EventModel>()
            .With(s => s.Name, 
                string.Join(string.Empty,fixture.CreateMany<char>(40)))
            .CreateMany()
            .ToArray();
        ICollection<EventModel> resultList = null;

        statisticsRepo
            .Setup(s => 
                s.GetAsync(statisticsItem.Id, CancellationToken.None))
            .ReturnsAsync(statisticsItem);
        eventsRepo
            .Setup(e =>
                e.CreateRangeAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None))
            .Callback((ICollection<EventModel> inEvents, CancellationToken _) =>
            {
                resultList = inEvents;
            });
       
        // Act
        await eventService.CreateRangeAsync(statisticsItem.Id, events);
        
        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.First().StatisticsId.Should().Be(statisticsItem.Id);
    }
    
    [Fact]
    public async Task Should_Not_Create_Events_Range_Statistics_Does_Not_Exists()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var statisticsRepo = new Mock<IStatisticsRepository>();
        var eventsRepo = new Mock<IEventRepository>();
        var eventService = new EventService(eventsRepo.Object, statisticsRepo.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();
        var events = fixture
            .Build<EventModel>()
            .With(s => s.Name, 
                string.Join(string.Empty,fixture.CreateMany<char>(55)))
            .CreateMany()
            .ToArray();
        ICollection<EventModel> resultList = null;
        
        eventsRepo
            .Setup(e =>
                e.CreateRangeAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None))
            .Callback((ICollection<EventModel> inEvents, CancellationToken _) =>
            {
                resultList = inEvents;
            });
       
        // Act
        try
        {
            await eventService.CreateRangeAsync(statisticsItem.Id, events);
            Assert.True(false, "Invalid data was written");
        }
        catch (ElementDoesNotExistsException)
        {
            Assert.True(true);
        }
    }
    
    [Fact]
    public async Task Should_Not_Create_Events_Range_Invalid_Data()
    {
        // Arrange
        var logger = new Mock<ILogger>();
        var statisticsRepo = new Mock<IStatisticsRepository>();
        var eventsRepo = new Mock<IEventRepository>();
        var eventService = new EventService(eventsRepo.Object, statisticsRepo.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();
        var events = fixture
            .Build<EventModel>()
            .With(s => s.Name, 
                string.Join(string.Empty,fixture.CreateMany<char>(55)))
            .CreateMany()
            .ToArray();
        ICollection<EventModel> resultList = null;

        statisticsRepo
            .Setup(s => 
                s.GetAsync(statisticsItem.Id, CancellationToken.None))
            .ReturnsAsync(statisticsItem);
        eventsRepo
            .Setup(e =>
                e.CreateRangeAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None))
            .Callback((ICollection<EventModel> inEvents, CancellationToken _) =>
            {
                resultList = inEvents;
            });
       
        // Act
        try
        {
            await eventService.CreateRangeAsync(statisticsItem.Id, events);
            Assert.True(false, "Invalid data was written");
        }
        catch (ArgumentException)
        {
            Assert.True(true);
        }
    }
}
