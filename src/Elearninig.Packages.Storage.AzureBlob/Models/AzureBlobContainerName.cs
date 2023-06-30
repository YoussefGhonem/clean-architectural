namespace Elearninig.Packages.Storage.AzureBlob.Models;

public class AzureBlobContainerName
{
    public AzureBlobContainerName(string name)
    {
        Name = name;
    }

    public string Name { get; }
}