using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Repositories;

public class StatisticsRepository : IStatisticsRepository
{
    private readonly IClientSessionHandle session;

    //private readonly DataContext dataContext;
    private readonly IMongoCollection<StatisticsModel> collection;

    public StatisticsRepository(IMongoDbContext mongoDbContext, IClientSessionHandle session)
    {
        this.session = session;
        //this.dataContext = dataContext;
        collection = mongoDbContext.GetStatisticsCollection();
    }
    
    public async Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        return await collection.AsQueryable(session).ToListAsync(token);
    }

    public Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        return collection.InsertOneAsync(session, statisticsModel, null,token);
    }
    public Task UpdateAsync(StatisticsModel newModel, CancellationToken token = default)
    {
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, newModel.Id);
        return collection.ReplaceOneAsync(session, searchFilter, newModel,new ReplaceOptions
        {
            IsUpsert = true
        }, token);
    }
    
    public Task<StatisticsModel?> GetAsync(Guid id, CancellationToken token = default)
    {
        var searchFilter = Builders<StatisticsModel>.Filter.Eq(f => f.Id, id);
        return collection.Find(session, searchFilter).FirstOrDefaultAsync(token);
    }
}