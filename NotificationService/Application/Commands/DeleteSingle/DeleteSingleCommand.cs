using Common.Extensions;
using FluentValidation;
using MediatR;

namespace NotificationService.Application.Commands.DeleteSingle
{
    public class DeleteSingleCommand : IRequest
    {
        public string NotificationId { get; init; }
    }

    public class DeleteSingleCommandValidator : AbstractValidator<DeleteSingleCommand>
    {
        public DeleteSingleCommandValidator()
        {
            RuleFor(x => x.NotificationId)
                .IsNotEmptyGuid();
        }
    }
}
