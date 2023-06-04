using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Base.API.ServiceCollections.HealthCheckApplication;
// Health checks are used in ASP.NET Core applications to monitor the health and availability of various components and
// dependencies of the application.They provide a way to check if critical services, resources, or external dependencies are functioning properly.
public static class ServiceCollectionExtension
{
    public static IServiceCollection AddHealthCheckApplication(this IServiceCollection services)
    {
        services.AddHealthChecks();
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
