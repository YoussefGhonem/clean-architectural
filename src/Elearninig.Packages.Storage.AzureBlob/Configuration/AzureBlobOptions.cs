namespace Elearninig.Packages.Storage.AzureBlob.Configuration;

public sealed class AzureBlobOptions
{
    public string? ConnectionString { get; set; }
    public string? DefaultContainer { get; set; }
}