using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class BankAccountNumberValidator : AbstractValidator<string>
    {
        public BankAccountNumberValidator()
        {
            const string regex = @"\d{2}[ ]\d{4}[ ]\d{4}[ ]\d{4}[ ]\d{4}[ ]\d{4}[ ]\d{4}|\d{26}";

            RuleFor(x => x)
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
