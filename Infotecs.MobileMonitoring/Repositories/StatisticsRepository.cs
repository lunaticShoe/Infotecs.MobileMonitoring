using System.Collections;
using System.Collections.Concurrent;
using System.Collections.ObjectModel;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly DataContext dataContext;
   
    public StatisticsRepository(DataContext dataContext)
    {
        this.dataContext = dataContext;
    }
    
    public async Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        var collection = await dataContext.PrepareStatisticsCollection(token);
        return await collection.AsQueryable().ToListAsync(token);
    }

    public async Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        var collection = await dataContext.PrepareStatisticsCollection(token);
        await collection.InsertOneAsync(statisticsModel, null,token);

    }
    public async Task UpdateAsync(StatisticsModel newModel, CancellationToken token = default)
    {
        var collection = await dataContext.PrepareStatisticsCollection(token);
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, newModel.Id);
        await collection.ReplaceOneAsync(searchFilter, newModel,new ReplaceOptions
        {
            IsUpsert = true
        }, token);
    }
    
    public async Task<StatisticsModel?> GetAsync(Guid id, CancellationToken token = default)
    {
        var collection = await dataContext.PrepareStatisticsCollection(token);
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, id);
        return await collection.Find(searchFilter).FirstOrDefaultAsync(token);
    }
}