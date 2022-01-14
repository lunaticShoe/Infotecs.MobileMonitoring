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
    //private readonly DataContext dataContext;
    private readonly IMongoCollection<StatisticsModel> collection;

    public StatisticsRepository(IMongoDbContext mongoDbContext)
    {
        //this.dataContext = dataContext;
        collection = mongoDbContext.GetStatisticsCollection();
    }
    
    public async Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        return await collection.AsQueryable().ToListAsync(token);
    }

    public Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        return collection.InsertOneAsync(statisticsModel, null,token);
    }
    public Task UpdateAsync(StatisticsModel newModel, CancellationToken token = default)
    {
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, newModel.Id);
        return collection.ReplaceOneAsync(searchFilter, newModel,new ReplaceOptions
        {
            IsUpsert = true
        }, token);
    }
    
    public Task<StatisticsModel?> GetAsync(Guid id, CancellationToken token = default)
    {
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, id);
        return collection.Find(searchFilter).FirstOrDefaultAsync(token);
    }
}