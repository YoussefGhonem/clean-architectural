using Elearninig.Base.Application.FluentValidation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Elearninig.Base.Application.FluentValidation.Validators
{
    public class VideoValidator : AbstractValidator<IFormFile>
    {
        private const int MaxSizeInMegaBytes = 16;

        public VideoValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .Must(BeVideo).WithMessage("Invalid file type, ' select only Video type '.")
                .Must(BeValidSize)
                .WithMessage($"Invalid Video size, ' size shouldn't be greater than {MaxSizeInMegaBytes} MB '.");
        }

        private bool BeVideo(IFormFile file)
        {
            return file.IsValidVideoExtension();
        }

        private bool BeValidSize(IFormFile file)
        {
            return !(file.SizeInMegabytes() > MaxSizeInMegaBytes);
        }
    }
}