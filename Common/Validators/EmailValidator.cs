using FluentValidation;

namespace Common.Validators
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
