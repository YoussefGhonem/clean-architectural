using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Base.Infrastructure.Extension;

// Initialise and seed database
public static class ApplicationDbContextInitialiser
{
    public static async Task MigrateDatabase(this IServiceScope scope)
    {

        #region DbContexts Migrations 

        #endregion

        #region for storage procedure
        // 'IHostingEnvironment' is an interface provided by ASP.NET Core that represents the hosting environment
        // in which the application is running. It provides information about the application's environment,
        // such as the content root path, environment name, and other related details.
        var hostingEnv = scope.ServiceProvider.GetRequiredService<IHostingEnvironment>();
        var path = hostingEnv.WebRootPath
                 + Path.DirectorySeparatorChar.ToString()
                 + "Scripts"
                 + Path.DirectorySeparatorChar.ToString();
        #endregion
    }

    public static async Task SeedDatabase(this IServiceScope scope, IHostingEnvironment env)
    {
    }
}
