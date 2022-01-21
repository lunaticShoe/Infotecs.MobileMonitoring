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
using MongoDB.Bson;
using Moq;
using Serilog;
using Xunit;

namespace Infotecs.MobileMonitoring.Tests.UnitTests.Services;

public class EventServiceTests
{
    private readonly Mock<ILogger> logger = new();
    private readonly Mock<ISessionContainer> container = new();
    private readonly Mock<ISessionContainerFactory> containerFactory = new();
    private readonly Mock<IStatisticsRepository> statisticsRepo = new();
    private readonly Mock<IEventRepository> eventsRepo = new();
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    private static readonly Fixture fixture = new();
    
    [Fact]
    public async Task Should_Create_Events_Range()
    {
        // Arrange
        CommonArrange();
        
        var eventService = new EventService(unitOfWork.Object, containerFactory.Object, logger.Object);
        
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
                e.CreateAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None))
            .Callback((ICollection<EventModel> inEvents, CancellationToken _) =>
            {
                resultList = inEvents;
            });
       
        // Act
        await eventService.CreateAsync(statisticsItem.Id, events);
        
        // Assert
        resultList.Should().NotBeNullOrEmpty();
        resultList.First().StatisticsId.Should().Be(statisticsItem.Id);
    }
    
    // [Fact]
    // public async Task Should_Not_Create_Events_Range_Statistics_Does_Not_Exists()
    // {
    //     // Arrange
    //     CommonArrange();
    //
    //     var eventService = new EventService(unitOfWork.Object, logger.Object);
    //     var statisticsItem = fixture.Create<StatisticsModel>();
    //     var events = fixture
    //         .Build<EventModel>()
    //         .With(s => s.Name, 
    //             string.Join(string.Empty,fixture.CreateMany<char>(55)))
    //         .CreateMany()
    //         .ToArray();
    //     
    //     eventsRepo
    //         .Setup(e =>
    //             e.CreateRangeAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None));
    //    
    //     // Act
    //     Func<Task> act = () => eventService.CreateRangeAsync(statisticsItem.Id, events);
    //     
    //     // Assert
    //     await act.Should().ThrowAsync<ElementDoesNotExistsException>("Invalid data was written");
    //     eventsRepo.Verify(e =>
    //         e.CreateRangeAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None), 
    //         () => Times.Never());   
    // }
    
    [Fact]
    public async Task Should_Not_Create_Events_Range_Invalid_Data()
    {
        // Arrange
        CommonArrange();
        
        var eventService = new EventService(unitOfWork.Object, containerFactory.Object, logger.Object);
        var statisticsItem = fixture.Create<StatisticsModel>();
        var events = fixture
            .Build<EventModel>()
            .With(s => s.Name, 
                string.Join(string.Empty,fixture.CreateMany<char>(55)))
            .CreateMany()
            .ToArray();
        //ICollection<EventModel> resultList = null;

        statisticsRepo
            .Setup(s => 
                s.GetAsync(statisticsItem.Id, CancellationToken.None))
            .ReturnsAsync(statisticsItem);
        eventsRepo
            .Setup(e =>
                e.CreateAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None));
       
        // Act
        Func<Task> act = () => eventService.CreateAsync(statisticsItem.Id, events);
        
        // Assert
        await act.Should().ThrowAsync<ArgumentException>("Invalid data was written");
        eventsRepo.Verify(e => 
            e.CreateAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None), 
            () => Times.Never());   
    }
    
    private void CommonArrange()
    {
        fixture.Register(ObjectId.GenerateNewId);
        unitOfWork
            .Setup(u => u.EventRepository)
            .Returns(eventsRepo.Object);
        unitOfWork
            .Setup(u => u.StatisticsRepository)
            .Returns(statisticsRepo.Object);
        containerFactory
            .Setup(cf => cf.Create(It.IsAny<IUnitOfWork>()))
            .Returns(container.Object);
    }

}
