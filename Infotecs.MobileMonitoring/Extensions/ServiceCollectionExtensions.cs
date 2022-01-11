using Serilog;

namespace Infotecs.MobileMonitoring.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddSerilogServices(
        this IServiceCollection services, 
        LoggerConfiguration configuration)
    {
        Log.Logger = configuration
            .MinimumLevel.Debug()
            .WriteTo
            .File(
                path: AppDomain.CurrentDomain.BaseDirectory + "/logs/log-.txt",
                retainedFileCountLimit: 5,
                shared: true,
                outputTemplate: "{Timestamp:dd.MM.yyyy HH:mm:ss.fff zzz} [{Level}] {Message:lj}{NewLine}{Exception}",
                rollingInterval: RollingInterval.Day
                )
            .CreateLogger();
        AppDomain.CurrentDomain.ProcessExit += (s, e) => Log.CloseAndFlush();
        return services.AddSingleton(Log.Logger);
    }
}