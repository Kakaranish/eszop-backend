using MediatR;
using System;
using System.Collections.Generic;
using Common.Dto;
using FluentValidation;

namespace Orders.API.Application.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid UserId { get; init; }
        public IList<CartItemDto> CartItems { get; init; }

        public CreateOrderCommand(CartDto cartDto)
        {
            UserId = cartDto.UserId;
            CartItems = cartDto.CartItems;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("Cannot be empty");

            RuleFor(x => x.CartItems)
                .NotNull();

            var cartItemValidator = new InlineValidator<CartItemDto>();
            cartItemValidator.RuleFor(x => x.OfferId)
                .NotEqual(Guid.Empty);
            cartItemValidator.RuleFor(x => x.OfferName)
                .NotNull()
                .NotEmpty();
            cartItemValidator.RuleFor(x => x.Quantity)
                .GreaterThan(0);
            cartItemValidator.RuleFor(x => x.PricePerItem)
                .GreaterThan(0);

            RuleForEach(x => x.CartItems)
                .SetValidator(cartItemValidator);
        }
    }
}
