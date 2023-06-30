using Elearninig.Packages.Storage.AzureBlob.Models;
using Elearninig.Packages.Storage.AzureBlob.Services.Storage;
using Microsoft.AspNetCore.Http;
using TicketManagement.Base.Helpers.Enums;
using TicketManagement.Base.Helpers.Models;

namespace Elearninig.Packages.Storage.AzureBlob.Services.MemorizeFileBlob
{
    public class MemorizeFileBlob : IMemorizeFile
    {
        private readonly IStorageService _storageService;

        public MemorizeFileBlob(IStorageService storageService)
        {
            _storageService = storageService;
        }

        public async Task<FileDto?> StoreFileInBlob(IFormFile? formFile, CancellationToken cancellationToken)
        {
            if (formFile is null) return null;

            #region Upload Image

            //store image in blob storage
            var storedImage =
                await _storageService.Upload(formFile, cancellationToken: cancellationToken);

            var uploadedImage = new FileDto
            {
                Id = storedImage.BlobId.ToString(),
                Url = storedImage.DownloadUrl,
                FileName = storedImage.FileName,
                FileSize = storedImage.FileSize,
                IsExternal = false
            };

            #endregion


            return uploadedImage;
        }

        public async Task<FileDto?> StoreFileInBlob(Stream? formFile, string? fileName, long? fileSize, CancellationToken cancellationToken)
        {
            if (formFile is null) return null;

            #region Upload Image

            //store image in blob storage
            var storedImage =
                await _storageService.Upload(formFile, fileName, cancellationToken: cancellationToken);

            var uploadedImage = new FileDto
            {
                Id = storedImage.BlobId.ToString(),
                Url = storedImage.DownloadUrl,
                FileName = fileName,
                FileSize = fileSize,
                IsExternal = false
            };

            #endregion


            return uploadedImage;
        }

        public async Task<List<(FileDto, string, FileTypeEnum, long)>?> StoreFiles(List<IFormFile>? files,
            CancellationToken cancellationToken)
        {
            if (files is null) return null;

            var storedFiles = await _storageService.UploadFiles(files, cancellationToken: cancellationToken);

            return storedFiles!.Select(f => (new FileDto
            {
                Id = f.BlobId.ToString(),
                Url = f.DownloadUrl!,
                FileName = f.FileName,
                FileSize = f.FileSize,
                IsExternal = false
            }, f.FileName is null ? "" : f.FileName, f.GetFileType(), f.FileSize)).ToList();
        }

        public async Task RollBackStorage(Guid id)
        {
            await _storageService.Delete(id);
        }
    }
}