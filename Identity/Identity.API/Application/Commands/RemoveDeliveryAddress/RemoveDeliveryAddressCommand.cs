using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Commands.RemoveDeliveryAddress
{
    public class RemoveDeliveryAddressCommand : IRequest
    {
        public string DeliveryAddressId { get; init; }
    }

    public class RemoveDeliveryAddressCommandValidator : AbstractValidator<RemoveDeliveryAddressCommand>
    {
        public RemoveDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.DeliveryAddressId)
                .IsNotEmptyGuid();
        }
    }
}
