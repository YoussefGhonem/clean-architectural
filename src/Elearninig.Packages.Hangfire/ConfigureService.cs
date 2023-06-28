using Elearninig.Packages.Hangfire.Configuration;
using Elearninig.Packages.Hangfire.Services.RecurringJob;
using Hangfire;
using Hangfire.SqlServer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Hangfire
{
    public static class ConfigureService
    {
        public static IServiceCollection AddHangFire(this IServiceCollection services, IConfiguration configuration)
        {
            var hangfireConfig = configuration.GetHangfireConfig();

            services.AddScoped<IRecurringJobService, RecurringJobService>();
            services.AddHangfire(x => x.UseSqlServerStorage(hangfireConfig.ConnectionString));
            services.AddHangfire((config) =>
            {
                var options = new SqlServerStorageOptions
                {
                    PrepareSchemaIfNecessary = true
                };
                config.UseSqlServerStorage(hangfireConfig.ConnectionString, options);
            });

            services.AddHangfireServer(a =>
            {
                a.SchedulePollingInterval = TimeSpan.FromHours(1);
                a.HeartbeatInterval = TimeSpan.FromHours(1);
                a.CancellationCheckInterval = TimeSpan.FromHours(1);
                a.ServerCheckInterval = TimeSpan.FromHours(1);
                {
                    a.ServerName = hangfireConfig.ServerName;
                }
                ;
            });

            GlobalConfiguration.Configuration.UseSqlServerStorage(hangfireConfig.ConnectionString,
                new SqlServerStorageOptions()
                {
                    QueuePollInterval = TimeSpan.FromHours(1),
                    CountersAggregateInterval = TimeSpan.FromHours(1),
                    JobExpirationCheckInterval = TimeSpan.FromHours(1),
                    SlidingInvisibilityTimeout = TimeSpan.FromHours(1),
                });

            services.AddHttpClient();
            services.AddHangfireServer();

            return services;
        }
        public static IApplicationBuilder UseHangFire(this IApplicationBuilder app)
        {
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = new[]
            {
                new HangfireAuthorizationFilter("SuperAdmin")
            }
            });
            return app;
        }
    }
}