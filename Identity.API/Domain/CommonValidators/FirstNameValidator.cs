using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class FirstNameValidator : AbstractValidator<string>
    {
        public FirstNameValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
