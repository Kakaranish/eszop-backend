using Common.Domain.Validators;
using FluentValidation;
using Identity.API.Services;
using MediatR;

namespace Identity.API.Application.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest
    {
        public string Email { get; init; }
        public string ResetToken { get; init; }
        public string NewPassword { get; init; }
    }

    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.NewPassword)
                .SetValidator(passwordValidator);

            RuleFor(x => x.ResetToken)
                .NotEmpty()
                .WithMessage("Cannot be empty");
        }
    }
}
