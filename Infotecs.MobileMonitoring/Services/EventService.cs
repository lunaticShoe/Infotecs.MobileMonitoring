using Infotecs.MobileMonitoring.Exceptions;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using ILogger = Serilog.ILogger;

namespace Infotecs.MobileMonitoring.Services;

public class EventService : IEventService
{
    private readonly IEventRepository eventRepository;
    private readonly IStatisticsRepository statisticsRepository;
    private readonly ILogger logger;

    public EventService(IEventRepository eventRepository, IStatisticsRepository statisticsRepository, ILogger logger)
    {
        this.eventRepository = eventRepository;
        this.statisticsRepository = statisticsRepository;
        this.logger = logger;
    }

    public async Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    {
        if (eventModel.StatisticsId == default)
            throw new Exception($"Invalid statistics Id");

        if (eventModel.Name.IsNullOrEmpty())
            throw new Exception("Event name is null or empty");
        
        var statisticsModel = await statisticsRepository.GetAsync(eventModel.StatisticsId, cancellationToken);

        if (statisticsModel is null)
            throw new Exception($"Element with id = {eventModel.StatisticsId} does not exists");
        
        if (eventModel.Name.Length > 50)
        {
            eventModel.Name = eventModel.Name[..50];
        }
        
        await eventRepository.CreateAsync(eventModel, cancellationToken);
        logger.Debug("Element added: {@Event}",eventModel);
    }

    public async Task CreateRangeAsync(Guid statisticsId, ICollection<EventModel> eventModels, CancellationToken cancellationToken = default)
    {
        if (statisticsId == default)
            throw new Exception($"Invalid statistics Id");

        var statisticsModel = await statisticsRepository.GetAsync(statisticsId, cancellationToken);

        if (statisticsModel is null)
            throw new ElementDoesNotExistsException(statisticsId);

        var hasBadEvents = eventModels
            .Any(e => e.Name.IsNullOrEmpty() || e.Name.Length > 50);
        
        if (hasBadEvents) 
            throw new ArgumentException("Some of event names are invalid");
        
        foreach (var eventModel in eventModels)
        {
            eventModel.StatisticsId = statisticsId;
        }
        await eventRepository.CreateRangeAsync(eventModels, cancellationToken);
        logger.Debug(
            "Elements for statistics id = {StatisticsId} added: {@Events}", 
            statisticsId, eventModels);
    }

    public Task<ICollection<EventModel>> GetListAsync(Guid statisticsId, CancellationToken cancellationToken)
    {
        return eventRepository.GetListAsync(statisticsId, cancellationToken);
    }
}
