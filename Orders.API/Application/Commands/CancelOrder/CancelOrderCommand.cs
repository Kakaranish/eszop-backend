using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Orders.API.Application.Commands.CancelOrder
{
    public class CancelOrderCommand : IRequest
    {
        public string OrderId { get; set; }
    }

    public class CancelOrderCommandValidator : AbstractValidator<CancelOrderCommand>
    {
        public CancelOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .IsGuid();
        }
    }
}
