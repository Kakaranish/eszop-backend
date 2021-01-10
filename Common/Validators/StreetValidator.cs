using FluentValidation;

namespace Common.Validators
{
    public class StreetValidator : AbstractValidator<string>
    {
        public StreetValidator()
        {
            const string regex = @"^[a-zA-Z\p{L}][a-zA-Z\s-\/0-9\p{L}]+$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
