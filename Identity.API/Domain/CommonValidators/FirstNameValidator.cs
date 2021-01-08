using FluentValidation;

namespace Identity.API.Domain.CommonValidators
{
    public class FirstNameValidator : AbstractValidator<string>
    {
        public FirstNameValidator()
        {
            const string regex = @"^[a-zA-Z\p{L}\s,.'-]{3,}$";

            RuleFor(x => x)
                .NotEmpty()
                .Matches(regex)
                .WithMessage($"Must match regex {regex}");
        }
    }
}
