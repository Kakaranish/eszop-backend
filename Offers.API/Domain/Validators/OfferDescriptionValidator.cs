using FluentValidation;

namespace Offers.API.Domain.Validators
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
