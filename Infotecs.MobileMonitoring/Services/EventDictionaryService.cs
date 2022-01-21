using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Serilog;

namespace Infotecs.MobileMonitoring.Services;

public class EventDictionaryService : IEventDictionaryService
{
    private readonly IUnitOfWork unitOfWork;
    private readonly ISessionContainerFactory sessionContainerFactory;
    private readonly ILogger logger;

    public EventDictionaryService(IUnitOfWork unitOfWork, ISessionContainerFactory sessionContainerFactory, ILogger logger)
    {
        this.unitOfWork = unitOfWork;
        this.sessionContainerFactory = sessionContainerFactory;
        this.logger = logger;
    }

    public async Task UpsertAsync(EventDictionaryModel model, CancellationToken cancellationToken = default)
    {
        var existingEvent = await unitOfWork.EventRepository.GetAsync(model.EventName, cancellationToken);
        if (existingEvent is null)
            throw new Exception($"No such event name {model.EventName}");

        using (sessionContainerFactory.Create(unitOfWork))
        {
            await unitOfWork.EventDictionaryRepository.UpsertAsync(model, cancellationToken);
            await unitOfWork.CommitAsync(cancellationToken);
        }
        logger.Debug("Description for event {Name} was created: {Model}", model.EventName, model);
    }

    public async Task<ICollection<EventDictionaryModel>> GetListAsync(CancellationToken cancellationToken = default)
    {
        var existingEvents = await unitOfWork.EventRepository
            .GetUniqueListAsync(cancellationToken);
        var eventsDictionary = await unitOfWork.EventDictionaryRepository
            .GetListAsync(cancellationToken);

        var result = from ee in existingEvents
                     join ed in eventsDictionary
                         on ee.Name equals ed.EventName into edList
                     from eventDict in edList.DefaultIfEmpty()
                     select new EventDictionaryModel
                     {
                         EventName = ee.Name,
                         Description = eventDict?.Description
                     };

        return result.ToArray();
    }
}
