using Infotecs.MobileMonitoring.Contracts;
using Infotecs.MobileMonitoring.Models;
using Mapster;
using Serilog;

namespace Infotecs.MobileMonitoring.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogServices(
        this IServiceCollection services, 
        LoggerConfiguration configuration,
        string seqConnection)
    {
        Log.Logger = configuration
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .WriteTo.Seq(seqConnection)
            // .File(
            //     path: AppDomain.CurrentDomain.BaseDirectory + "/logs/log-.txt",
            //     retainedFileCountLimit: 5,
            //     shared: true,
            //     outputTemplate: "{Timestamp:dd.MM.yyyy HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}",
            //     rollingInterval: RollingInterval.Day
            //     )
            .CreateLogger();
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
        return services.AddSingleton(Log.Logger);
    }

    public static IServiceCollection AddMapsterConfiguration(this IServiceCollection services)
    {
        TypeAdapterConfig<StatisticsContract, StatisticsModel>
            .ForType()
            .BeforeMapping(model =>
            {
                model.CreatedAt = DateTime.Now;
            });
        return services;
    }

    public static IServiceCollection AddNSwag(this IServiceCollection services)
    {
        services.AddOpenApiDocument(doc => 
            doc.PostProcess = document =>
            {
                document.Info.Title = "Infotecs Monitoring Service API";
            });
        //builder.Services.AddOpenApiDocument();
        //builder.Services.AddSwaggerDocument();
        // builder.Services.AddSwaggerGen(c =>
        // {
        //     c.SwaggerDoc("v1", new OpenApiInfo
        //     {
        //         Version = "v1",
        //         Title = "Infotecs Monitoring Service",
        //         Description = "Infotecs Monitoring Service API",
        //     });
        //     // Set the comments path for the Swagger JSON and UI.
        //     var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        //     var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        //     c.IncludeXmlComments(xmlPath);
        // });
        return services;
    }
}