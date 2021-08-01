using Common.Domain.Validators;
using FluentValidation;
using Identity.API.Application.Dto;
using MediatR;

namespace Identity.API.Application.Commands.SignIn
{
    public class SignInCommand : IRequest<TokenResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator()
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.Password)
                .NotNull()
                .NotEmpty();
        }
    }
}
