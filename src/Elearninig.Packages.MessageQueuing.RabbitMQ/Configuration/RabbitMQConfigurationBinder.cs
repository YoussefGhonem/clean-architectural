using Microsoft.Extensions.Configuration;

namespace Elearninig.Packages.MessageQueuing.RabbitMQ.Configuration;
public static class RabbitMQConfigurationBinder
{
    public static RabbitMQOptions GetRabbitMQConfig(this IConfiguration configuration)
    {
        var config = configuration.GetSection("RabbitMQ").Get<RabbitMQOptions>();
        if (config is null)
        {
            throw new Exception("Missing 'RabbitMQ' configuration section from the appsettings.");
        }

        return config;
    }
}