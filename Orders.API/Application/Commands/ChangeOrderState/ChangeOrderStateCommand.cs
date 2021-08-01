using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Orders.Domain.Aggregates.OrderAggregate;
using System.Collections.Generic;

namespace Orders.API.Application.Commands.ChangeOrderState
{
    public class ChangeOrderStateCommand : IRequest
    {
        public string OrderId { get; set; }
        public string OrderState { get; init; }
    }

    public class ChangeOrderStateCommandValidator : AbstractValidator<ChangeOrderStateCommand>
    {
        public ChangeOrderStateCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();

            var validStates = new List<string>
            {
                OrderState.InProgress.Name,
                OrderState.InPreparation.Name,
                OrderState.Shipped.Name
            };
            RuleFor(x => x.OrderState)
                .Must(x => validStates.Contains(x))
                .WithMessage("Illegal state");
        }
    }
}
