﻿using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Carts.API.Application.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommand : IRequest
    {
        public string CartItemId { get; init; }
        public int Quantity { get; init; }
    }

    public class UpdateCartItemQuantityCommandValidator : AbstractValidator<UpdateCartItemQuantityCommand>
    {
        public UpdateCartItemQuantityCommandValidator()
        {
            RuleFor(x => x.CartItemId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
