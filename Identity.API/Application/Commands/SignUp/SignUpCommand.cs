using Common.Validators;
using FluentValidation;
using Identity.API.Application.Dto;
using Identity.API.Domain.CommonValidators;
using Identity.API.Services;
using MediatR;

namespace Identity.API.Application.Commands.SignUp
{
    public class SignUpCommand : IRequest<TokenResponse>
    {
        public string Email { get; init; }
        public string Password { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
    }

    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator(PasswordValidatorBase passwordValidator)
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());

            RuleFor(x => x.Password)
                .SetValidator(passwordValidator);

            RuleFor(x => x.FirstName)
                .SetValidator(new FirstNameValidator());

            RuleFor(x => x.LastName)
                .SetValidator(new LastNameValidator());
        }
    }
}
