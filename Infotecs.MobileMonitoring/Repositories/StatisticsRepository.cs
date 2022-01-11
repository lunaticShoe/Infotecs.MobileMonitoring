using System.Collections;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly List<StatisticsModel> _statisticsModels = new();

    public Task<IEnumerable<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
        {
            return Task.FromResult(Enumerable.Empty<StatisticsModel>());
        }

        return Task.FromResult(_statisticsModels.AsEnumerable());
    }

    public Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        if (token.IsCancellationRequested)
        {
            return Task.CompletedTask;
        }
        
        _statisticsModels.Add(statisticsModel);
        
        return Task.CompletedTask;
    }
    
    public Task<StatisticsModel?> Get(Guid id, CancellationToken token = default)
    {
        token.ThrowIfCancellationRequested();
        return Task.FromResult(_statisticsModels.FirstOrDefault(sm => sm.Id == id));
    }
}