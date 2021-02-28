using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Carts.API.Application.Commands.RemoveFromCart
{
    public class RemoveFromCartCommand : IRequest
    {
        public string CartItemId { get; init; }
    }

    public class RemoveFromCartCommandValidation : AbstractValidator<RemoveFromCartCommand>
    {
        public RemoveFromCartCommandValidation()
        {
            RuleFor(x => x.CartItemId)
                .IsNotEmptyGuid();
        }
    }
}
