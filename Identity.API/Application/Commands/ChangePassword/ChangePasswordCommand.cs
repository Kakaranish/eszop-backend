using FluentValidation;
using Identity.API.Services;
using MediatR;

namespace Identity.API.Application.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest
    {
        public string OldPassword { get; init; }
        public string NewPassword { get; init; }
    }

    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator(PasswordValidatorBase passwordValidator)
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
