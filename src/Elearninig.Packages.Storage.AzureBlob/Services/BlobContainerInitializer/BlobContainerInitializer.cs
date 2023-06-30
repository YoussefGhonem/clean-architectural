using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elearninig.Packages.Storage.AzureBlob.Configuration;
using Microsoft.Extensions.Configuration;
using System.Collections.Concurrent;
using TicketManagement.Base.Helpers.Extensions;

namespace Elearninig.Packages.Storage.AzureBlob.Services.BlobContainerInitializer;

public class BlobContainerInitializer : IBlobContainerInitializer
{
    private readonly string _connectionString;

    private readonly ConcurrentDictionary<string, bool> _existsCache = new();
    //private readonly AzureConfig _storageConfig;

    public BlobContainerInitializer(IConfiguration configuration)
    {
        //_storageConfig = storageConfig;
        // var confg = configuration.GetSection("AzureBlobStorage").Get<AzureConfig>();
        var confg = configuration.GetEnvironmentSection<AzureBlobOptions>("AzureBlobStorage");

        if (confg == null)
            throw new Exception("Missing 'AzureBlobStorage' configuration section from the appsettings.");
        _connectionString = confg.ConnectionString ?? throw new Exception("connection string should not be null");
        //_connectionString = _storageConfig.ConnectionString ?? throw new Exception("connection string should not be null");
    }

    public void EnsureContainerExists(string blobContainerName)
    {
        if (_existsCache.ContainsKey(blobContainerName))
            return;

        var serviceClient = new BlobServiceClient(_connectionString);
        try
        {
            serviceClient.CreateBlobContainer(blobContainerName, PublicAccessType.None);
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == "ContainerAlreadyExists")
        {
            // It already exists, do nothing
        }

        _existsCache[blobContainerName] = true;
    }
}