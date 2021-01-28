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
        public decimal? Price { get; init; }
    }

    public class UpdatePredefinedDeliveryMethodCommandValidator : AbstractValidator<UpdatePredefinedDeliveryMethodCommand>
    {
        public UpdatePredefinedDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.DeliveryMethodId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Price)
                .Must(x => x == null || x >= 0)
                .WithMessage("Must be null or >= 0");
        }
    }
}
