using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly ConcurrentDictionary<Guid, StatisticsModel> statisticsModels = new();

    public Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
        {
            return Task.FromResult(Array.Empty<StatisticsModel>() as ICollection<StatisticsModel>);
        }

        return Task.FromResult(
            new ReadOnlyCollection<StatisticsModel>(
                statisticsModels
                    .Select(kv => kv.Value)
                    .ToList()) as ICollection<StatisticsModel>);
    }

    public Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        statisticsModels.TryAdd(statisticsModel.Id,statisticsModel);
        return Task.CompletedTask;
    }
    
    public Task<StatisticsModel?> GetAsync(Guid id, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        if (statisticsModels.TryGetValue(id, out var model))
            return Task.FromResult(model);
        return Task.FromResult<StatisticsModel?>(null);
    }
}