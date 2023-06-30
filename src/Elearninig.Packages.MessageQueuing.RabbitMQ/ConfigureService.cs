using Elearninig.Packages.MessageQueuing.RabbitMQ.Configuration;
using GreenPipes;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Net;
using System.Reflection;
using System.Text;
using System.Web;

namespace Elearninig.Packages.MessageQueuing.RabbitMQ;

public static class ConfigureService
{
    public static IServiceCollection AddRabbitMQ(this IServiceCollection services, IConfiguration configuration)
    {
        CreateVirtualHostIfNotExist(configuration);
        services.AddMassTransitConfiguration(configuration);
        return services;
    }

    private static IServiceCollection AddMassTransitConfiguration(this IServiceCollection services,
        IConfiguration configuration)
    {
        var config = configuration.GetRabbitMQConfig();

        services.Configure<HealthCheckPublisherOptions>(options =>
        {
            options.Delay = TimeSpan.FromSeconds(2);
            options.Predicate = (check) => check.Tags.Contains("ready");
        });

        services.AddMassTransit(x =>
        {
            x.AddConsumers(GetAssemblies());
            x.AddDelayedMessageScheduler();
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(config.Host, (ushort)config.Port, config.VirtualHost,
                    opt =>
                    {
                        opt.Username(config.Username);
                        opt.Password(config.Password);
                    });
                // add the delayed message scheduler for the redelivery functionality
                x.AddDelayedMessageScheduler();
                // Immediate retry
                cfg.UseRetry(r =>
                {
                    r.Immediate(config.NumberOfRetry);
                    r.Ignore(typeof(InvalidOperationException), typeof(InvalidCastException));
                });

                if (config.UseExponential)
                {
                    cfg.UseRetry(r =>
                    {
                        // retry will have an exponential function
                        r.Exponential(config.NumberOfRetry, TimeSpan.FromSeconds(1), TimeSpan.FromDays(1),
                            TimeSpan.FromMinutes(1));

                        // any message that will throw this types of exceptions the message will not be tried again
                        r.Ignore(typeof(InvalidOperationException), typeof(InvalidCastException));
                    });
                }

                if (config.UseScheduledRedelivery)
                {
                    // For redelivery the message gets removed from the queue and then delivered at a later time
                    // MassTransit uses the RabbitMQ delayed exchange plug-in to schedule messages
                    cfg.UseDelayedMessageScheduler();
                    cfg.UseScheduledRedelivery(r => r.Intervals(TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(15),
                        TimeSpan.FromMinutes(30)));
                }
                // configure the rotes that the consumers will receives the messages on
                cfg.ConfigureEndpoints(context);
            });
        });

        // Adds mass transit services
        services.AddMassTransitHostedService();
        return services;
    }

    // Create virtual host if not exist
    public static void CreateVirtualHostIfNotExist(IConfiguration configuration)
    {
        var config = configuration.GetRabbitMQConfig();
        var credentials = new NetworkCredential() { UserName = config.Username, Password = config.Password };
        using var handler = new HttpClientHandler { Credentials = credentials };
        using var client = new HttpClient(handler);
        var urlEncoded = config.VirtualHost == "/" ? "" : HttpUtility.UrlEncode(config.VirtualHost);
        var url = $"http://{config.Host}:15672/api/vhosts/{urlEncoded}";
        var content = new StringContent("", Encoding.UTF8, "application/json");
        var response = client.GetAsync(url).Result;

        // if virtual host not exist create one
        if ((int)response.StatusCode >= 300)
        {
            var result = client.PutAsync(url, content).Result;

            if ((int)result.StatusCode >= 300)
                throw new Exception(result.ToString());
        }
    }

    // Get the entry assembly and all its references that start with the same base name (ex. FortTeck) and then loads them
    private static Assembly[] GetAssemblies()
    {
        var entryAssembly = Assembly.GetEntryAssembly();
        if (entryAssembly == null)
            return Array.Empty<Assembly>();

        var assemblies = entryAssembly
            .GetReferencedAssemblies()
            .Where(a => !string.IsNullOrEmpty(a.FullName)
                        && entryAssembly.FullName != null
                        && a.FullName.StartsWith(entryAssembly.FullName.Split('.')[0]))
            .Select(Assembly.Load).ToList();
        assemblies.Add(entryAssembly);
        return assemblies.ToArray();
    }
}