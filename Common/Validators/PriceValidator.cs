using FluentValidation;

namespace Common.Validators
{
    public class PriceValidator : AbstractValidator<decimal>
    {
        public PriceValidator()
        {
            RuleFor(x => x)
                .GreaterThan(0);
        }
    }
}
