using FluentValidation;

namespace Elearninig.Base.Application.FluentValidation.Validators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(b => b)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(255)
                .EmailAddress().WithMessage("Your email address (({PropertyValue})) isn't valid");
        }
    }
}