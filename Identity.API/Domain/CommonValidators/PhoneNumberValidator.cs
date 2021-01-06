using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class PhoneNumberValidator : AbstractValidator<string>
    {
        public PhoneNumberValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .Matches(@"(?<!\w)(\(?(\+|00)?48\)?)?[ -]?\d{3}[ -]?\d{3}[ -]?\d{3}(?!\w)");
        }
    }
}
