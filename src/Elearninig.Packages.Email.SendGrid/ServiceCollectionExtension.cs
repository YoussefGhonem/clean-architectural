using Elearninig.Packages.Email.Base.Interfaces;
using Elearninig.Packages.Email.SendGrid.Configuration;
using Elearninig.Packages.Email.SendGrid.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Email.SendGrid;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSendGrid(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSendGridConfig();

        services.AddSingleton<IEmailService, EmailService>();
        return services;
    }
}