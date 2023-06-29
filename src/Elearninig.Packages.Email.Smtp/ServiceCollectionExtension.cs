using Elearninig.Packages.Email.Base.Interfaces;
using Elearninig.Packages.Email.Smtp.Configuration;
using Elearninig.Packages.Email.Smtp.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Email.Smtp;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddSmtpWithMicrosoftKit(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetSmtpConfig();

        services.AddSingleton<IEmailService, EmailService>();

        return services;
    }
}