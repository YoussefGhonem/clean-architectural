using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Base.API.ServiceCollections.HealthCheckApplication;
// Health checks are used in ASP.NET Core applications to monitor the health and availability of various components and
// dependencies of the application.They provide a way to check if critical services, resources, or external dependencies are functioning properly.
// هو هنا بيغير شكل الريسبونس بتاعك وبيخليه يطلعلك شكل الاريرور اوضح وملهوش
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHealthCheckApplication(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
    {
        services.AddHealthChecks()
            .AddSqlServer(configuration.GetConnectionString("DefaultConnection")!);
        return services;
    }
    public static WebApplication UseHealthCheckApplication(this WebApplication app)
    {
        app.UseHealthChecks("/health-checks-ui", new HealthCheckOptions
        {
            Predicate = _ => true, //  It accepts any request
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse // to write the health check response.to write the health check response.
        });
        app.MapHealthChecksUI();
        return app;
    }
}
