using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoFixture;
using FluentAssertions;
using Infotecs.MobileMonitoring.Dto;
using Infotecs.MobileMonitoring.Exceptions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Infotecs.MobileMonitoring.Repositories;
using Infotecs.MobileMonitoring.Services;
using Mapster;
using MongoDB.Bson;
using Moq;
using Serilog;
using Xunit;

namespace Infotecs.MobileMonitoring.Tests.UnitTests.Services;

public class StatisticsServiceTests
{
    private readonly Mock<ILogger> logger = new();
    private readonly Mock<ISessionContainer> container = new();
    private readonly Mock<ISessionContainerFactory> containerFactory = new();
    private readonly Mock<IStatisticsRepository> statisticsRepo = new();
    private readonly Mock<IEventRepository> eventsRepo = new();
    //private readonly Mock<IEventService> eventService = new();
    private EventService eventService = null;
    private readonly Mock<IUnitOfWork> unitOfWork = new();
    
    private static readonly Fixture fixture = new();
    private StatisticsService statisticsService;
    private StatisticsDto? statisticsItem;

    [Fact]
    public async Task Should_Create_Statistics_Item()
    {
        // Arrange
        CommonArrange();
        
        StatisticsModel resultModel = null;
        ICollection<EventModel> resultEvents = null;
        
        statisticsRepo
            .Setup(s =>
                s.CreateAsync(It.IsAny<StatisticsModel>(), CancellationToken.None))
            .Callback((StatisticsModel a, CancellationToken _) => { resultModel = a; });
        eventsRepo.Setup(e =>
                e.CreateAsync(It.IsAny<ICollection<EventModel>>(), CancellationToken.None))
            .Callback((ICollection<EventModel> events, CancellationToken _) => { resultEvents = events; });
            
        // Act
        await statisticsService.CreateAsync(statisticsItem);
        // Assert
        resultModel.Should().NotBeNull();
        resultModel.Should().BeEquivalentTo(statisticsItem.Adapt<StatisticsModel>());
        resultEvents.Should().BeEquivalentTo(statisticsItem.Events);
    }
    
    [Fact]
    public async Task Should_NotCreateExistingStatisticsItem_Throws()
    {
        // Arrange
        CommonArrange();

        statisticsRepo
            .Setup(s => 
                s.GetAsync(statisticsItem.Id, CancellationToken.None))
            .ReturnsAsync(statisticsItem.Adapt<StatisticsModel>());
        
        // Act
        Func<Task> act = () => statisticsService.CreateAsync(statisticsItem, CancellationToken.None);
        
        // Assert
        await act.Should()
            .ThrowAsync<ElementAlreadyExistsException>("Invalid statistics item created");

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

        eventService = new EventService(unitOfWork.Object, containerFactory.Object, logger.Object);
        statisticsService = new StatisticsService(unitOfWork.Object, containerFactory.Object, 
            eventService, logger.Object);
        statisticsItem = fixture.Create<StatisticsDto>();
    }
}
