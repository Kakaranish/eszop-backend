using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class EmailValidator : AbstractValidator<string>
    {
        public EmailValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .EmailAddress();
        }
    }
}
