using Common.Dto;
using Common.Extensions;
using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;

namespace Orders.API.Application.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public Guid BuyerId { get; }
        public Guid SellerId { get; }
        public IList<CreateOrderCartItemDto> CartItems { get; }

        public CreateOrderCommand(CreateOrderCartDto createOrderCartDto)
        {
            BuyerId = createOrderCartDto.UserId;
            SellerId = createOrderCartDto.SellerId;
            CartItems = createOrderCartDto.CartItems;
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

            var cartItemValidator = new InlineValidator<CreateOrderCartItemDto>();
            cartItemValidator.RuleFor(x => x.OfferId)
                .NotEqual(Guid.Empty);
            cartItemValidator.RuleFor(x => x.OfferName)
                .NotNull()
                .NotEmpty();
            cartItemValidator.RuleFor(x => x.Quantity)
                .GreaterThan(0);
            cartItemValidator.RuleFor(x => x.PricePerItem)
                .GreaterThan(0);
            cartItemValidator.RuleFor(x => x.ImageUri)
                .Must(x => !string.IsNullOrWhiteSpace(x) && Uri.IsWellFormedUriString(x, UriKind.Absolute))
                .WithMessage("Invalid uri");

            RuleForEach(x => x.CartItems)
                .SetValidator(cartItemValidator);
        }
    }
}
