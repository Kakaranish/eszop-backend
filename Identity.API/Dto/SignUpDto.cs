using FluentValidation;
using Identity.API.Services;

namespace Identity.API.Dto
{
    public class SignUpDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class SignUpDtoValidator : AbstractValidator<SignUpDto>
    {
        public SignUpDtoValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Password)
                .SetValidator(passwordValidator);

            RuleFor(x => x.FirstName)
                .NotNull()
                .MinimumLength(3);

            RuleFor(x => x.LastName)
                .NotNull()
                .MinimumLength(3);
        }
    }
}
