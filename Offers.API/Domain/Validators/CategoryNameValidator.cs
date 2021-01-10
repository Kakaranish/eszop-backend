using FluentValidation;

namespace Offers.API.Domain.Validators
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
