using Microsoft.Extensions.Configuration;
using TicketManagement.Base.Helpers.Extensions;

namespace Elearninig.Packages.Storage.AzureBlob.Configuration;

public static class AzureBlobConfigurationBinder
{
    public static AzureBlobOptions GetAzureConfig(this IConfiguration configuration)
    {
        var confg = configuration.GetEnvironmentSection<AzureBlobOptions>("AzureBlobStorage");
        if (confg == null)
            throw new Exception("Missing 'AzureBlobStorage' configuration section from the appsettings.");
        return confg;
    }
}