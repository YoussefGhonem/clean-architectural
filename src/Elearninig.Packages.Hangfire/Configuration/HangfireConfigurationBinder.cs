using Microsoft.Extensions.Configuration;

namespace Elearninig.Packages.Hangfire.Configuration;

public static class HangfireConfigurationBinder
{
    public static HangfireOptions GetHangfireConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("Hangfire").Get<HangfireOptions>();
        if (config is null)
        {
            throw new Exception("Missing 'Hangfire' configuration section from the appsettings.");
        }

        config.ConnectionString = configuration.GetSection("ConnectionStrings").GetValue<string>("DefaultConnection");

        return config;
    }
}