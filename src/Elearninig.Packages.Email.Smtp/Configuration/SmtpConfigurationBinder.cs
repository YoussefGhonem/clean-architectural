using Elearninig.Packages.Email.Smtp.Config;
using Microsoft.Extensions.Configuration;

namespace Elearninig.Packages.Email.Smtp.Configuration;

public static class SmtpConfigurationBinder
{
    public static SmtpOptions GetSmtpConfig(this IConfiguration configuration)
    {
        var confg = configuration.GetSection("MailProviders:Smtp").Get<SmtpOptions>();
        if (confg == null)
            throw new Exception("Missing 'Smtp' configuration section from the appsettings.");
        return confg;
    }
}