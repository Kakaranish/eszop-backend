using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Commands.LockUser
{
    public class LockUserCommand : IRequest
    {
        public string UserId { get; set; }
    }

    public class LockUserCommandValidator : AbstractValidator<LockUserCommand>
    {
        public LockUserCommandValidator()
        {
            RuleFor(x => x.UserId)
                .IsNotEmptyGuid();
        }
    }
}
