using FluentValidation;

namespace Common.Domain.Validators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .EmailAddress();
        }
    }
}
