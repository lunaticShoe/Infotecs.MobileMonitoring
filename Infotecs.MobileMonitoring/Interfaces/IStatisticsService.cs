using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IStatisticsService
{
    Task<IEnumerable<StatisticsModel>> GetListAsync(CancellationToken token = default);

    Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default);
}