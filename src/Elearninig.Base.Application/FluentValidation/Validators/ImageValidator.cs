using Elearninig.Base.Application.FluentValidation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Elearninig.Base.Application.FluentValidation.Validators
{
    public class ImageValidator : AbstractValidator<IFormFile>
    {
        private const int MaxSizeInMegaBytes = 1;

        public ImageValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .Must(BeImage).WithMessage("Invalid file type, ' select only image type '.")
                .Must(BeValidSize)
                .WithMessage($"Invalid image size, ' size shouldn't be greater than {MaxSizeInMegaBytes} MB '.");
        }

        private bool BeValidSize(IFormFile file)
        {
            return !(file.SizeInMegabytes() > MaxSizeInMegaBytes);
        }

        private bool BeImage(IFormFile file)
        {
            return file.IsValidImageExtension();
        }
    }
}