using Microsoft.AspNetCore.Http;

namespace Elearninig.Base.Application.FluentValidation.Extensions
{
    public static class FileExtensions
    {
        public static readonly List<string> DocumentExtensions = new()
            { "pdf" };
        public static readonly List<string> ImageExtensions = new()
            {"jpeg", "jpg", "png", "gif", "svg", "heic", "raw", "jfif"};

        public static readonly List<string> VideoExtensions = new()
            {"mp4", "m4p", "m4v", "webm", "mov", "ogg", "mbg", "mb2", "mbeg", "mbe", "mbv", "avi"};

        public static readonly List<string> AudioExtensions = new()
            { "wav", "aif", "mp3", "mid"};

        public static string Extension(this IFormFile file)
        {
            return file.FileName.ToLower().Split('.').Last();
        }

        public static bool IsValidImageExtension(this IFormFile file)
        {
            return ImageExtensions.Contains(file.Extension());
        }

        public static bool IsValidVideoExtension(this IFormFile file)
        {
            return VideoExtensions.Contains(file.Extension());
        }

        public static bool IsValidAudioExtension(this IFormFile file)
        {
            return AudioExtensions.Contains(file.Extension());
        }

        public static bool IsValidDocumentExtension(this IFormFile file)
        {
            return DocumentExtensions.Contains(file.Extension());
        }

        public static bool IsImageOrVideo(this IFormFile file)
        {
            var extension = file.Extension();

            if (ImageExtensions.Contains(extension) || VideoExtensions.Contains(extension))
                return true;
            return false;
        }

        public static long SizeInMegabytes(this IFormFile file)
        {
            return file.Length / (1024 * 1024);
        }

        public static long SizeInKilobytes(this IFormFile file)
        {
            return file.Length / 1024;
        }

    }
}