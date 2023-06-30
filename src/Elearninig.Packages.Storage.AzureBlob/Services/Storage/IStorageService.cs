using Elearninig.Packages.Storage.AzureBlob.Models;
using Microsoft.AspNetCore.Http;

namespace Elearninig.Packages.Storage.AzureBlob.Services.Storage;

public interface IStorageService
{
    Task<StoredFile> Upload(IFormFile file, string? containerName = null,
        CancellationToken cancellationToken = default);

    Task<StoredFile> Upload(Stream? file, string? fileName, string? containerName = null,
        CancellationToken cancellationToken = default);

    Task<List<StoredFile>?> UploadFiles(List<IFormFile>? file, string? containerName = null,
        CancellationToken cancellationToken = default);

    Task Delete(Guid blobId, string? containerName = null);
    Task<DownloadedFile> Download(Guid blobId, string? containerName = null);
    Task<byte[]?> DownlaodAsMemoryStream(Guid blobId, string? containerName = null);
}