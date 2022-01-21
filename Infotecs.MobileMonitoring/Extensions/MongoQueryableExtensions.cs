using System;
using MongoDB.Driver.Linq;

namespace Infotecs.MobileMonitoring.Extensions;

public static class MongoQueryableExtensions
{
    public static IMongoQueryable<T> If<T>(
        this IMongoQueryable<T> source,
        bool condition,
        Func<IMongoQueryable<T>, IMongoQueryable<T>> transform)
    {
        return condition ? transform(source) : source;
    }

}
