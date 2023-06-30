using Azure;
using Azure.Storage.Blobs.Models;

namespace Elearninig.Packages.Storage.AzureBlob.Models;

public class DownloadedFile
{
    public Stream Contents { get; }
    public string FileName { get; }
    public string ContentType { get; }
    public long ContentLength { get; }

    public DownloadedFile(Response<BlobDownloadInfo> downloadInfo, Response<BlobProperties> blobProperties)
    {
        Contents = downloadInfo.Value.Content;
        FileName = blobProperties.Value.Metadata[BlobMetadata.FILE_NAME];
        ContentType = downloadInfo.Value.ContentType;
        ContentLength = downloadInfo.Value.ContentLength;
    }
}