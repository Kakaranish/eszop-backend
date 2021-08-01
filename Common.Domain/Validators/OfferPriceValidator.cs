using FluentValidation;

namespace Common.Domain.Validators
{
    public class OfferPriceValidator : AbstractValidator<decimal>
    {
        public OfferPriceValidator()
        {
            RuleFor(x => x)
                .GreaterThan(0);
        }
    }
}
