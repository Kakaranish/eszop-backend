using FluentValidation;
using MediatR;
using System;

namespace Offers.API.Application.Commands.CreatePredefinedDeliveryMethod
{
    public class CreatePredefinedDeliveryMethodCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string Description { get; init; }
    }

    public class CreatePredefinedDeliveryMethodCommandValidator :
        AbstractValidator<CreatePredefinedDeliveryMethodCommand>
    {
        public CreatePredefinedDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();
        }
    }
}
