using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Orders.API.Application.Commands.ConfirmOrder
{
    public class ConfirmOrderCommand : IRequest
    {
        [FromRoute] public string OrderId { get; init; }
    }

    public class ConfirmOrderCommandValidator : AbstractValidator<ConfirmOrderCommand>
    {
        public ConfirmOrderCommandValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
