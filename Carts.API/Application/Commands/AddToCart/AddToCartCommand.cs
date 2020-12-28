using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Carts.API.Application.Commands.AddToCart
{
    public class AddToCartCommand : IRequest
    {
        public string OfferId { get; init; }
        public int Quantity { get; init; }
    }

    public class AddToCartCommandValidator : AbstractValidator<AddToCartCommand>
    {
        public AddToCartCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsGuid();
            
            RuleFor(x => x.Quantity)
                .GreaterThan(0);
        }
    }
}
