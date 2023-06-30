using Elearninig.Packages.Storage.AzureBlob.Configuration;
using Elearninig.Packages.Storage.AzureBlob.Models;
using Elearninig.Packages.Storage.AzureBlob.Services.BlobContainerInitializer;
using Elearninig.Packages.Storage.AzureBlob.Services.MemorizeFileBlob;
using Elearninig.Packages.Storage.AzureBlob.Services.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Elearninig.Packages.Storage.AzureBlob;

public static class ConfigureService
{
    public static IServiceCollection AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
    {
        configuration.GetAzureConfig();

        services.AddSingleton<IBlobContainerInitializer, BlobContainerInitializer>();
        services.AddSingleton(new AzureBlobContainerName("documents-new"));
        services.AddSingleton<IStorageService, StorageService>();
        services.AddSingleton<IMemorizeFile, MemorizeFileBlob>();

        return services;
    }
}