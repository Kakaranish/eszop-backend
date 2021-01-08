using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Identity.API.Application.Commands.SetPrimaryDeliveryAddress
{
    public class SetPrimaryDeliveryAddressCommand : IRequest
    {
        public string DeliveryAddressId { get; init; }
    }

    public class SetPrimaryDeliveryAddressCommandValidator : AbstractValidator<SetPrimaryDeliveryAddressCommand>
    {
        public SetPrimaryDeliveryAddressCommandValidator()
        {
            RuleFor(x => x.DeliveryAddressId)
                .IsGuid();
        }
    }
}
