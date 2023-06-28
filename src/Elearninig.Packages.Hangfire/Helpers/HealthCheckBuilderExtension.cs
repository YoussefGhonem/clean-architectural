using HealthChecks.Hangfire;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Hangfire.Helpers
{
    public static class HealthCheckBuilderExtension
    {
        public static IHealthChecksBuilder AddHangfireHealthChecks(
        this IHealthChecksBuilder healthChecksBuilder,
        IConfiguration configuration)
        {
            var hangFireOptions = new HangfireOptions
            {
                MaximumJobsFailed = 5,
                MinimumAvailableServers = 1,
            };
            healthChecksBuilder.AddHangfire(x => x = hangFireOptions, name: "hangfire-check", tags: new string[] { "hangfire" });
            return healthChecksBuilder;
        }
    }
}
