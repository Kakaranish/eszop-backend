using FluentValidation;

namespace Common.Validators
{
    public class CountryValidator : AbstractValidator<string>
    {
        public CountryValidator()
        {
            const string regex = @"^[A-Z\p{L}][a-z\p{L}]+(\s[A-Z\p{L}][a-z\p{L}]+)*$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
