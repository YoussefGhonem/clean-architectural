using Elearninig.Base.Application.FluentValidation.Extensions;
using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace Elearninig.Base.Application.FluentValidation.Validators
{
    public class AudioValidator : AbstractValidator<IFormFile>
    {
        private const int MaxSizeInMegaBytes = 5;

        public AudioValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .Must(BeAudio).WithMessage("Invalid file type, ' select only audio type '.")
                .Must(BeValidSize)
                .WithMessage($"Invalid adio size, ' size shouldn't be greater than {MaxSizeInMegaBytes} MB '.");
        }

        private bool BeValidSize(IFormFile file)
        {
            return !(file.SizeInMegabytes() > MaxSizeInMegaBytes);
        }

        private bool BeAudio(IFormFile file)
        {
            return file.IsValidAudioExtension();
        }
    }
}
