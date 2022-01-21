using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Infotecs.MobileMonitoring.Data;
using Infotecs.MobileMonitoring.Extensions;
using Infotecs.MobileMonitoring.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infotecs.MobileMonitoring.Migrator;

internal class MigrationVersion
{
    public ObjectId Id { get; set; }
    public int Version { get; set; }
    public DateTime MigratedAt { get; set; }
}

internal class MigrationType
{
    public int Version { get; set; }
    public Type Type { get; set; }
}

public class MongoDbMigrator : IMongoDbMigrator
{
    private readonly IMongoDbContext context;

    public MongoDbMigrator(IMongoDbContext context)
    {
        this.context = context;
    }
    
    private IMongoCollection<MigrationVersion> MigrationCollection => 
        context.Database
        .GetCollection<MigrationVersion>("__migrationVersion");

    public void Migrate()
    {
        var version = GetLastDbVersion();
        var migrations = GetMigrations();

        if (migrations.IsNullOrEmpty()) return;

        int startMigrationIndex = 0;

        if (version is not null)
        {
            startMigrationIndex = migrations
                .FindIndex(t => t.Version == version);

            if (startMigrationIndex < 0) return;
            
            startMigrationIndex++;
        }

        for (int i = startMigrationIndex; i < migrations.Count; i++)
        {
            MigrateOne(migrations[i]);
        }

    }

    private void MigrateOne(MigrationType migrationType)
    {
        var migrationInstance = Activator
            .CreateInstance(migrationType.Type) as IMongoDbMigration;
        migrationInstance!.Migrate(context);
        
        MigrationCollection.InsertOne(new MigrationVersion
        {
            Version = migrationType.Version,
            MigratedAt = DateTime.UtcNow
        });
    }
    
    private int? GetLastDbVersion()
    {
        var lastMigration = MigrationCollection
            .AsQueryable()
            .OrderByDescending(mv => mv.Version)
            .FirstOrDefault();

        return lastMigration?.Version;
    }
    
    private List<MigrationType> GetMigrations()
    {
        var migrationType = typeof(IMongoDbMigration);
        return AppDomain.CurrentDomain
            .GetAssemblies()
            .SelectMany(s => s.GetTypes())
            .Where(t => migrationType.IsAssignableFrom(t))
            .Select(t => FilterTypeName(t))
            .Where(mt => mt is { })
            .DistinctBy(mt => mt!.Version)
            .OrderBy(mt => mt!.Version)
            .ToList()!;
    }

    private MigrationType? FilterTypeName(Type type)
    {
        var nameUnderscoresIndex = type.Name
            .LastIndexOf("__", StringComparison.Ordinal);
        
        if (nameUnderscoresIndex < 0) return null;
        
        string versionString = type.Name[(nameUnderscoresIndex + 2)..];
        if (string.IsNullOrEmpty(versionString)) return null;
        if (!int.TryParse(versionString, out int version)) return null;
        
        return new MigrationType { Version = version, Type = type };
    }
}
