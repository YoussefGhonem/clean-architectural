using Azure;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Elearninig.Packages.Storage.AzureBlob.Configuration;
using Elearninig.Packages.Storage.AzureBlob.Extensions;
using Elearninig.Packages.Storage.AzureBlob.Models;
using Elearninig.Packages.Storage.AzureBlob.Services.BlobContainerInitializer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Elearninig.Packages.Storage.AzureBlob.Services.Storage;

public class StorageService : IStorageService
{
    private readonly AzureBlobOptions _storageConfig;
    private readonly BlobServiceClient _blobServiceClient;
    private readonly string _blobContainerName;

    public StorageService(IConfiguration configuration,
        AzureBlobContainerName azureBlobContainerName,
        IBlobContainerInitializer blobContainerInitializer)
    {
        _storageConfig = configuration.GetAzureConfig();
        _blobServiceClient = new BlobServiceClient(_storageConfig.ConnectionString);

        _blobContainerName = azureBlobContainerName.Name;

        blobContainerInitializer.EnsureContainerExists(_blobContainerName);
    }

    public async Task<StoredFile> Upload(IFormFile file, string? containerName = null,
        CancellationToken cancellationToken = default)
    {
        var blobId = Guid.NewGuid();
        var stream = file.OpenReadStream();
        var blobFileName = GetFileName(blobId, file.FileName);
        var metadata = file.GetBlobMetadata(blobFileName);


        if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
        containerName = containerName!.EditeContainerName();
        CreateContainerIfNotExist(containerName);

        var blobClient = _blobServiceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobId.ToString());

        await blobClient.UploadAsync(stream, new BlobUploadOptions { Metadata = metadata }, cancellationToken);
        var storedFile = new StoredFile
        {
            BlobId = blobId,
            FileName = file.FileName,
            FileSize = file.Length,
            UploadedDate = DateTimeOffset.Now,
            DownloadUrl = blobClient.Uri.AbsoluteUri,
        };
        return storedFile;
    }

    public async Task<StoredFile> Upload(Stream? file, string? fileName, string? containerName = null,
        CancellationToken cancellationToken = default)
    {
        if (file is null) return new StoredFile();
        if (fileName is null) return new StoredFile();

        var blobId = Guid.NewGuid();
        var stream = file;
        var blobFileName = GetFileName(blobId, fileName);
        var metadata = file.GetBlobMetadata(blobFileName);


        if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
        containerName = containerName!.EditeContainerName();
        CreateContainerIfNotExist(containerName);

        var blobClient = _blobServiceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobId.ToString());

        await blobClient.UploadAsync(stream, new BlobUploadOptions { Metadata = metadata }, cancellationToken);
        var storedFile = new StoredFile
        {
            BlobId = blobId,
            UploadedDate = DateTimeOffset.Now,
            DownloadUrl = blobClient.Uri.AbsoluteUri,
        };
        return storedFile;
    }

    public async Task<List<StoredFile>?> UploadFiles(List<IFormFile>? files, string? containerName = null,
        CancellationToken cancellationToken = default)
    {
        if (files is null) return null;
        var storedFiles = new List<StoredFile>();
        foreach (var file in files)
        {
            var blobId = Guid.NewGuid();
            var stream = file.OpenReadStream();
            var blobFileName = GetFileName(blobId, file.ContentType);
            var metadata = file.GetBlobMetadata(blobFileName);

            if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
            containerName = containerName!.EditeContainerName();
            CreateContainerIfNotExist(containerName!);

            var blobClient = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobId.ToString());

            await blobClient.UploadAsync(stream, new BlobUploadOptions { Metadata = metadata }, cancellationToken);
            var storedFile = new StoredFile
            {
                BlobId = blobId,
                FileName = file.FileName,
                FileSize = file.Length,
                UploadedDate = DateTimeOffset.Now,
                DownloadUrl = blobClient.Uri.AbsoluteUri,
            };
            storedFiles.Add(storedFile);
        }

        return storedFiles;
    }


    public async Task Delete(Guid blobId, string? containerName = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
            containerName = containerName!.EditeContainerName();
            CreateContainerIfNotExist(containerName!);

            var blobClient = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobId.ToString());

            await blobClient.DeleteAsync();
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            // Already gone
        }
    }

    public async Task<BlobClient> GetFile(Guid blobId, string? containerName = null)
    {
        if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
        containerName = containerName!.EditeContainerName();
        CreateContainerIfNotExist(containerName!);

        var blobClient = _blobServiceClient
            .GetBlobContainerClient(containerName)
            .GetBlobClient(blobId.ToString());
        return blobClient;
    }

    public async Task<DownloadedFile> Download(Guid blobId, string? containerName = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
            containerName = containerName!.EditeContainerName();
            CreateContainerIfNotExist(containerName!);

            var blobClient = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobId.ToString());

            var properties = await blobClient.GetPropertiesAsync();
            var response = await blobClient.DownloadAsync();

            return new DownloadedFile(response, properties);
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new Exception($"File with id '{blobId}' not found");
        }
    }

    public async Task<byte[]?> DownlaodAsMemoryStream(Guid blobId, string? containerName = null)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(containerName)) containerName = _storageConfig.DefaultContainer;
            containerName = containerName!.EditeContainerName();
            CreateContainerIfNotExist(containerName!);

            var blobClient = _blobServiceClient
                .GetBlobContainerClient(containerName)
                .GetBlobClient(blobId.ToString());

            using var memoryStream = new MemoryStream();

            await blobClient.DownloadToAsync(memoryStream);
            return memoryStream.ToArray();
        }
        catch (RequestFailedException ex) when (ex.Status == 404)
        {
            throw new Exception($"File with id '{blobId}' not found");
        }
    }

    private void CreateContainerIfNotExist(string container)
    {
        try
        {
            var blobContainers = _blobServiceClient.GetBlobContainers();
            var containerExists = blobContainers.Any(x =>
                string.Equals(x.Name, container?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (!containerExists)
            {
                _blobServiceClient.CreateBlobContainer(container, PublicAccessType.None);
            }
        }
        catch (RequestFailedException ex) when (ex.ErrorCode == "ContainerAlreadyExists")
        {
            // It already exists, do nothing
        }
    }

    private static string GetFileName(Guid blobId, string contentType)
    {
        var fileSplit = contentType.Split('.');
        var fileExtension = fileSplit[^1];
        var blobFileName = blobId.ToString();
        if (fileSplit.Length > 1) blobFileName += "." + fileExtension;
        return blobFileName;
    }
}