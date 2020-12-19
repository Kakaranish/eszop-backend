using FluentValidation;
using Identity.API.Services;

namespace Identity.API.Dto
{
    public class ChangePasswordDto
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class ChangePasswordDtoValidator : AbstractValidator<ChangePasswordDto>
    {
        public ChangePasswordDtoValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.OldPassword)
                .NotEmpty()
                .WithMessage("Cannot be empty");

            RuleFor(x => x.OldPassword)
                .Must((model, oldPassword) => model.NewPassword != oldPassword)
                .WithMessage("Old password and new password cannot be the same");

            RuleFor(x => x.NewPassword)
                .SetValidator(passwordValidator);
        }
    }
}
