using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Exceptions;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using ILogger = Serilog.ILogger;

namespace Infotecs.MobileMonitoring.Services;

public class EventService : IEventService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ISessionContainerFactory sessionContainerFactory;
    private readonly ILogger logger;

    public EventService(IUnitOfWork unitOfWork, ISessionContainerFactory sessionContainerFactory, ILogger logger)
    {
        // this.eventRepository = eventRepository;
        // this.statisticsRepository = statisticsRepository;
        this.unitOfWork = unitOfWork;
        this.sessionContainerFactory = sessionContainerFactory;
        this.logger = logger;
    }

    // public async Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    // {
    //     if (eventModel.StatisticsId == default)
    //         throw new Exception($"Invalid statistics Id");
    //
    //     if (eventModel.Name.IsNullOrEmpty())
    //         throw new Exception("Event name is null or empty");
    //     
    //     var statisticsModel = await unitOfWork.StatisticsRepository.GetAsync(eventModel.StatisticsId, cancellationToken);
    //
    //     if (statisticsModel is null)
    //         throw new Exception($"Element with id = {eventModel.StatisticsId} does not exists");
    //     
    //     if (eventModel.Name.Length > 50)
    //     {
    //         eventModel.Name = eventModel.Name[..50];
    //     }
    //     
    //     await unitOfWork.EventRepository.CreateAsync(eventModel, cancellationToken);
    //     logger.Debug("Element added: {@Event}",eventModel);
    // }

    public async Task CreateAsync(Guid statisticsId, ICollection<EventModel> eventModels, CancellationToken cancellationToken = default)
    {
        var hasBadEvents = eventModels
            .Any(e => e.Name.IsNullOrEmpty() || e.Name.Length > 50);
        
        if (hasBadEvents) 
            throw new ArgumentException("Some of event names are invalid");
        
        foreach (var eventModel in eventModels)
        {
            eventModel.StatisticsId = statisticsId;
        }
        await unitOfWork.EventRepository.CreateAsync(eventModels, cancellationToken);
        logger.Debug(
            "Elements for statistics id = {StatisticsId} added: {@Events}", 
            statisticsId, eventModels);
    }

    public Task<ICollection<EventModel>> GetListAsync(Guid statisticsId, CancellationToken cancellationToken)
    {
        return unitOfWork.EventRepository.GetListAsync(statisticsId, cancellationToken);
    }

    public async Task DeleteAsync(Guid statisticsId, CancellationToken cancellationToken)
    {
        using (sessionContainerFactory.Create(unitOfWork))
        {
            await unitOfWork.EventRepository.DeleteAsync(statisticsId, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
    }
}
