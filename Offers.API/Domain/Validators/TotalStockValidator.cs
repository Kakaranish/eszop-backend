using FluentValidation;

namespace Offers.API.Domain.Validators
{
    public class TotalStockValidator : AbstractValidator<int>
    {
        public TotalStockValidator()
        {
            RuleFor(x => x)
                .GreaterThan(0);
        }
    }
}
