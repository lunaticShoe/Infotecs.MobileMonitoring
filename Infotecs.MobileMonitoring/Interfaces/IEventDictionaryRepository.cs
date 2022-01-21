using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IEventDictionaryRepository
{
    Task UpsertAsync(EventDictionaryModel model, CancellationToken cancellationToken = default);
    Task<ICollection<EventDictionaryModel>> GetListAsync(CancellationToken cancellationToken = default);
}
