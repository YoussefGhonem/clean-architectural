namespace Elearninig.Packages.Storage.AzureBlob.Services.BlobContainerInitializer;

public interface IBlobContainerInitializer
{
    void EnsureContainerExists(string blobContainerName);
}