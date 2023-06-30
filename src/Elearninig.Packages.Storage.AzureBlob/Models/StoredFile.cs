namespace Elearninig.Packages.Storage.AzureBlob.Models;

public class StoredFile
{
    public Guid BlobId { get; set; }
    public string? FileName { get; set; }
    public string? DownloadUrl { get; set; }
    public DateTimeOffset UploadedDate { get; set; }
    public long FileSize { get; set; }
}