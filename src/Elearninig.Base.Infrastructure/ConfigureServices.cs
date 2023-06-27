using Elearninig.Base.Infrastructure.Persistence;
using Elearninig.Base.Infrastructure.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Base.Infrastructure
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();
            services.AddDbContext<ApplicationDbContext>(opt =>
            opt.UseSqlServer(
                configuration.GetConnectionString("DefaultConnection"),
                b => b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));
            return services;
        }
    }
}
