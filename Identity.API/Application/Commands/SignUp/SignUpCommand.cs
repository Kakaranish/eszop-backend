using Common.Validators;
using FluentValidation;
using Identity.API.Application.Dto;
using Identity.API.Services;
using MediatR;

namespace Identity.API.Application.Commands.SignUp
{
    public class SignUpCommand : IRequest<TokenResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.Password)
                .SetValidator(passwordValidator);
        }
    }
}
