using FluentValidation;

namespace Offers.Domain.Validators
{
    public class OfferDescriptionValidator : AbstractValidator<string>
    {
        public OfferDescriptionValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .MinimumLength(5);
        }
    }
}
