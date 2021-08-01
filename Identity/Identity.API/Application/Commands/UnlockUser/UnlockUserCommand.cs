using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Commands.UnlockUser
{
    public class UnlockUserCommand : IRequest
    {
        public string UserId { get; init; }
    }

    public class UnlockUserCommandValidator : AbstractValidator<UnlockUserCommand>
    {
        public UnlockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .IsNotEmptyGuid();
        }
    }
}
