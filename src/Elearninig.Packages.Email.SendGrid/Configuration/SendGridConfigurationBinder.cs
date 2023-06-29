using Microsoft.Extensions.Configuration;

namespace Elearninig.Packages.Email.SendGrid.Configuration;

public static class ConfigurationExtension
{
    public static SendGridOptions GetSendGridConfig(this IConfiguration configuration)
    {
        var confg = configuration.GetSection("MailProviders:SendGrid").Get<SendGridOptions>();
        if (confg == null)
            throw new Exception("Missing 'SendGrid' configuration section from the appsettings.");
        return confg;
    }
}