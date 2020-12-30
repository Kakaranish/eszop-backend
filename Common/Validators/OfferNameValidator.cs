using FluentValidation;

namespace Common.Validators
{
    public class OfferNameValidator : AbstractValidator<string>
    {
        public OfferNameValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
