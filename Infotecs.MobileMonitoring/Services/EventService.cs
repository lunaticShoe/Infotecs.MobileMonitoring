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
        
        var existingItem = await statisticsRepository.GetAsync(eventModel.StatisticsId, cancellationToken);

        if (existingItem is null)
            throw new Exception($"Element with id = {eventModel.StatisticsId} does not exists");
        
        if (eventModel.Name.Length > 50)
        {
            eventModel.Name = eventModel.Name[..50];
        }
        
        await eventRepository.CreateAsync(eventModel, cancellationToken);
        logger.Debug("Element added: {@EventModel}",eventModel);
    }

    public Task<ICollection<EventModel>> GetListAsync(Guid statisticsId, CancellationToken cancellationToken)
    {
        return eventRepository.GetListAsync(statisticsId, cancellationToken);
    }
}
