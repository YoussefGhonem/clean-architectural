using Microsoft.AspNetCore.Http;

namespace Elearninig.Packages.Storage.AzureBlob.Models;

public static class BlobMetadata
{
    public const string FILE_NAME = "original_filename";
    private const string OriginalNameMetadata = "original_name";
    private const string OriginalFilenameMetadata = "original_filename";

    public static Dictionary<string, string> GetBlobMetadata(this IFormFile file, string fileName)
    {
        return new Dictionary<string, string>()
        {
            //{FILE_NAME, file.FileName},
            {OriginalFilenameMetadata, fileName},
            {OriginalNameMetadata, fileName},
        };
    }

    public static Dictionary<string, string> GetBlobMetadata(this Stream file, string fileName)
    {
        return new Dictionary<string, string>()
        {
            //{FILE_NAME, file.FileName},
            {OriginalFilenameMetadata, fileName},
            {OriginalNameMetadata, fileName},
        };
    }
}