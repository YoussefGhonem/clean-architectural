using TicketManagement.Base.Helpers.Enums;
using TicketManagement.Base.Helpers.Extensions;

namespace Elearninig.Packages.Storage.AzureBlob.Models
{
    public static class StoredFileExtension
    {
        public static FileTypeEnum GetFileType(this StoredFile file)
        {
            var extension = file.FileName.ToLower().Split('.').Last();

            if (FileExtensions.ImageExtensions.Contains(extension) is true)
                return FileTypeEnum.Image;

            else if (FileExtensions.VideoExtensions.Contains(extension) is true)
                return FileTypeEnum.Video;

            else if (FileExtensions.AudioExtensions.Contains(extension) is true)
                return FileTypeEnum.Audio;

            return FileTypeEnum.NotValid;
        }
    }
}