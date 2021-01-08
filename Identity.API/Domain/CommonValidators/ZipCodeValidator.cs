using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class ZipCodeValidator : AbstractValidator<string>
    {
        public ZipCodeValidator()
        {
            const string regex = @"^\d{1,6}(-\d{1,6})?$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
