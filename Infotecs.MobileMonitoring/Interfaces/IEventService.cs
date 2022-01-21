using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IEventService
{
    //Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default);
    Task<ICollection<EventModel>> GetListAsync(Guid statisticsId, CancellationToken cancellationToken);
    Task CreateAsync(Guid statisticsId, ICollection<EventModel> eventModels, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid statisticsId, CancellationToken cancellationToken);
}
