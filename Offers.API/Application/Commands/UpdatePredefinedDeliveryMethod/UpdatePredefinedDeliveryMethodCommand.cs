using Common.Extensions;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.UpdatePredefinedDeliveryMethod
{
    public class UpdatePredefinedDeliveryMethodCommand : IRequest
    {
        public string DeliveryMethodId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
    }

    public class UpdatePredefinedDeliveryMethodCommandValidator : AbstractValidator<UpdatePredefinedDeliveryMethodCommand>
    {
        public UpdatePredefinedDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.DeliveryMethodId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
