using FluentValidation;
using Identity.API.Services;

namespace Identity.API.Dto
{
    public class ResetPasswordDto
    {
        public string ResetToken { get; set; }
        public string NewPassword { get; set; }
    }

    public class ResetPasswordDtoValidator : AbstractValidator<ResetPasswordDto>
    {
        public ResetPasswordDtoValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.NewPassword)
                .SetValidator(passwordValidator);

            RuleFor(x => x.ResetToken)
                .NotEmpty()
                .WithMessage("Cannot be empty");
        }
    }
}
