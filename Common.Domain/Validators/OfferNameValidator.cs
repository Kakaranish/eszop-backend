using FluentValidation;

namespace Common.Domain.Validators
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
