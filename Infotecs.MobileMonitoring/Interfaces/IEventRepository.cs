using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IEventRepository
{
    Task<ICollection<EventModel>> GetListAsync(Guid? statisticsId = null, CancellationToken cancellationToken = default);
    Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default);
}
