using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Orders.API.Application.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public string OrderId { get; init; }
    }

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
