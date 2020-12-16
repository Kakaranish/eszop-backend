using FluentValidation;

namespace Identity.API.Dto
{
    public class SignInDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
    public class SignInDtoValidator : AbstractValidator<SignInDto>
    {
        public SignInDtoValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
