using Microsoft.AspNetCore.Http;
using TicketManagement.Base.Helpers.Enums;
using TicketManagement.Base.Helpers.Models;

namespace Elearninig.Packages.Storage.AzureBlob.Services.MemorizeFileBlob
{
    public interface IMemorizeFile
    {
        public Task<FileDto?> StoreFileInBlob(IFormFile? formFile, CancellationToken cancellationToken);
        public Task<FileDto?> StoreFileInBlob(Stream? formFile, string? fileName, long? fileSize, CancellationToken cancellationToken);
        public Task<List<(FileDto, string, FileTypeEnum, long)>?> StoreFiles(List<IFormFile>? files,
            CancellationToken cancellationToken);

        Task RollBackStorage(Guid Id);
    }
}