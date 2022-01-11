using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IStatisticsRepository
{
    /// <summary>
    /// Получить список элементов статистики
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<IEnumerable<StatisticsModel>> GetListAsync(CancellationToken token = default);

    /// <summary>
    /// Добавить элемент статистики в хранилище
    /// </summary>
    /// <param name="statisticsModel"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default);

    Task<StatisticsModel?> Get(Guid id, CancellationToken token = default);
}