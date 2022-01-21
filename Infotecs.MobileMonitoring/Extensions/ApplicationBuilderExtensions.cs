using Microsoft.AspNetCore.Builder;

namespace Infotecs.MobileMonitoring.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IApplicationBuilder UseSwaggerInfrastructure(this IApplicationBuilder app)
    {
        // app.UseSwagger();
        // app.UseSwaggerUI();
        // app.UseOpenApi(c =>
        // {
        //     c.
        // });
        app.UseOpenApi();
        app.UseSwaggerUi3();
        return app;
    }
}
