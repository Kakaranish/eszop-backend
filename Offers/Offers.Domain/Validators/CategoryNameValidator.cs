using FluentValidation;

namespace Offers.Domain.Validators
{
    public class CategoryNameValidator : AbstractValidator<string>
    {
        public CategoryNameValidator()
        {
            RuleFor(x => x)
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
