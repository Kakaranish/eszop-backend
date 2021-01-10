using FluentValidation;

namespace Common.Validators
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
