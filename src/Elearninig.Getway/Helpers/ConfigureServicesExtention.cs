
namespace Elearninig.Getway.Helpers;
public static class ConfigureServicesExtention
{
    // At run time, the ConfigureServices method is called before the Configure method.
    // It includes built-in IoC container
    public static IServiceCollection ConfigureServices(this IServiceCollection services)
    {
        return services;
    }

    // Introduced the middleware components to define a request pipeline, which will be executed on every request.
    public static IApplicationBuilder Configure(this IApplicationBuilder app, IWebHostEnvironment env)
    {
        return app;
    }
}
