using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Interfaces;

public interface IStatisticsRepository
{
    /// <summary>
    /// Получить список элементов статистики
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default);

    /// <summary>
    /// Добавить элемент статистики в хранилище
    /// </summary>
    /// <param name="statisticsModel"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default);

    Task<StatisticsModel?> GetAsync(Guid id, CancellationToken token = default);
    Task UpdateAsync(StatisticsModel newModel, CancellationToken token = default);
}