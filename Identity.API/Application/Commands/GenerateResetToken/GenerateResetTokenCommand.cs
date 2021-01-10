using Common.Validators;
using FluentValidation;
using Identity.API.Domain.CommonValidators;
using MediatR;

namespace Identity.API.Application.Commands.GenerateResetToken
{
    public class GenerateResetTokenCommand : IRequest<string>
    {
        public string Email { get; init; }
    }

    public class GenerateResetTokenCommandValidator : AbstractValidator<GenerateResetTokenCommand>
    {
        public GenerateResetTokenCommandValidator()
        {
            RuleFor(x => x.Email)
                .SetValidator(new EmailValidator());
        }
    }
}
