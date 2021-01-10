using FluentValidation;

namespace Common.Validators
{
    public class CityValidator : AbstractValidator<string>
    {
        public CityValidator()
        {
            const string regex = @"^[a-zA-Z\p{L}]+(?:[\s-][a-zA-Z\p{L}]+)*$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
