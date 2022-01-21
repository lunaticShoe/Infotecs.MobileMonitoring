using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infotecs.MobileMonitoring.Repositories;

public class EventRepository : IEventRepository
{
    private readonly IClientSessionHandle session;

    //private readonly DataContext dataContext;
    private readonly IMongoCollection<EventModel> collection;

    public EventRepository(IMongoDbContext mongoDbContext, IClientSessionHandle session)
    {
        this.session = session;
        collection = mongoDbContext.GetEventCollection();
    }

    public async Task<ICollection<EventModel>> GetListAsync(Guid? statisticsId = null, CancellationToken cancellationToken = default)
    {
        return await collection
            .AsQueryable(session)
            .If(!statisticsId.IsNullOrEmpty(), 
                c => c
                    .Where(eventModel => eventModel.StatisticsId == statisticsId!))
            .ToListAsync(cancellationToken);
    }

    public async Task<ICollection<EventModel>> GetUniqueListAsync(CancellationToken cancellationToken = default)
    {
        return await collection
            .AsQueryable(session)
            .GroupBy(x => x.Name)
            .Select(x => new EventModel
            {
                Name = x.Key
            })
         //  .If(name is not null, )
            .ToListAsync(cancellationToken);
        // return (await collection.DistinctAsync(session, x => x.Name, FilterDefinition<EventModel>.Empty, null, cancellationToken))
        //     .ToListAsync(cancellationToken);
    }

    public Task<EventModel> GetAsync(string name, CancellationToken cancellationToken = default)
    {
        return collection
            .AsQueryable(session)
            .Where(x => x.Name == name)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    {
        return collection.InsertOneAsync(session, eventModel, null,cancellationToken);
    }

    public Task CreateAsync(ICollection<EventModel> events, CancellationToken cancellationToken = default)
    {
        return collection.InsertManyAsync(session, events,null, cancellationToken);
    }

    public Task DeleteAsync(Guid statisticsId, CancellationToken cancellationToken = default)
    {
        var searchFilter = Builders<EventModel>.Filter.Eq(f => f.StatisticsId, statisticsId);
        return collection.DeleteManyAsync(session, searchFilter, null, cancellationToken);
    }
    
    
}
