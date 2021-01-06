using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class LastNameValidator : AbstractValidator<string>
    {
        public LastNameValidator()
        {
            RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);
        }
    }
}
