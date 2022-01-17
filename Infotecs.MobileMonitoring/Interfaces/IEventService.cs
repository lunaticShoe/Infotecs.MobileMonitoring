using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IEventService
{
    Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default);
    Task<ICollection<EventModel>> GetListAsync(Guid statisticsId, CancellationToken cancellationToken);
    Task CreateRangeAsync(Guid statisticsId, ICollection<EventModel> eventModels, CancellationToken cancellationToken = default);
}
