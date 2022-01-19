using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Infotecs.MobileMonitoring.Repositories;

public class EventRepository : IEventRepository
{
    //private readonly DataContext dataContext;
    private readonly IMongoCollection<EventModel> collection;

    public EventRepository(IMongoDbContext mongoDbContext)
    {
      //  this.dataContext = dataContext;
        collection = mongoDbContext.GetEventCollection();
    }

    public async Task<ICollection<EventModel>> GetListAsync(Guid? statisticsId = null, CancellationToken cancellationToken = default)
    {
        return await collection
            .AsQueryable()
            .If(!statisticsId.IsNullOrEmpty(), 
                c => c
                    .Where(eventModel => eventModel.StatisticsId == statisticsId!))
            .ToListAsync(cancellationToken);
    }

    public Task CreateAsync(EventModel eventModel, CancellationToken cancellationToken = default)
    {
        return collection.InsertOneAsync(eventModel, null,cancellationToken);
    }

    public Task CreateRangeAsync(ICollection<EventModel> events, CancellationToken cancellationToken = default)
    {
        return collection.InsertManyAsync(events,null, cancellationToken);
    }
}
