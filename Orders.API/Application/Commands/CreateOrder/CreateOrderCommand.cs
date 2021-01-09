using MediatR;
using System;
using System.Collections.Generic;
using Common.Dto;
using Common.Extensions;
using FluentValidation;

namespace Orders.API.Application.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid BuyerId { get; }
        public Guid SellerId { get; }
        public IList<CartItemDto> CartItems { get; }

        public CreateOrderCommand(CartDto cartDto)
        {
            BuyerId = cartDto.UserId;
            SellerId = cartDto.SellerId;
            CartItems = cartDto.CartItems;
        }
    }

    public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
    {
        public CreateOrderCommandValidator()
        {
            RuleFor(x => x.BuyerId)
                .IsNotEmptyGuid();

            RuleFor(x => x.SellerId)
                .IsNotEmptyGuid();

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
