using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IEventRepository
{
    Task<ICollection<EventModel>> GetListAsync(Guid? statisticsId = null, CancellationToken cancellationToken = default);
    Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default);
    Task CreateAsync(ICollection<EventModel> events, CancellationToken cancellationToken = default);
    Task<ICollection<EventModel>> GetUniqueListAsync(CancellationToken cancellationToken = default);
    Task<EventModel> GetAsync(string name, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid statisticsId, CancellationToken cancellationToken = default);
}
