using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IStatisticsService
{
    Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default);

    Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default);
    Task UpdateAsync(StatisticsModel statisticsModel, CancellationToken token = default);
}