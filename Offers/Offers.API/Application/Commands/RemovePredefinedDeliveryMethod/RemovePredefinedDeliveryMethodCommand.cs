using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.RemovePredefinedDeliveryMethod
{
    public class RemovePredefinedDeliveryMethodCommand : IRequest
    {
        public string DeliveryMethodId { get; init; }
    }

    public class RemovePredefinedDeliveryMethodCommandValidator : 
        AbstractValidator<RemovePredefinedDeliveryMethodCommand>
    {
        public RemovePredefinedDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.DeliveryMethodId)
                .IsNotEmptyGuid();
        }
    }
}
