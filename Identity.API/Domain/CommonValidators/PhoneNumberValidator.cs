using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class PhoneNumberValidator : AbstractValidator<string>
    {
        public PhoneNumberValidator()
        {
            const string regex = @"^[0-9\+\-\s]{3,}$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
