using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Repositories;

public class EventDictionaryRepository : IEventDictionaryRepository
{
    private readonly IClientSessionHandle session;
    private readonly IMongoCollection<EventDictionaryModel> collection;

    public EventDictionaryRepository(IMongoDbContext mongoDbContext, IClientSessionHandle session)
    {
        this.session = session;
        collection = mongoDbContext.GetEventDictionaryCollection();
    }

    public Task UpsertAsync(EventDictionaryModel model, CancellationToken cancellationToken = default)
    {
        var searchFilter = Builders<EventDictionaryModel>.Filter.Eq(f => f.EventName, model.EventName);
        return collection.ReplaceOneAsync(session, searchFilter, model, new ReplaceOptions
        {
            IsUpsert = true
        },cancellationToken);
    }
    
    public async Task<ICollection<EventDictionaryModel>> GetListAsync(CancellationToken cancellationToken = default)
    {
        return await collection.AsQueryable(session).ToListAsync(cancellationToken);
    }
}
