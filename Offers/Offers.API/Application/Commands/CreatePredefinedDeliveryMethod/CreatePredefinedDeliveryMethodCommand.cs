using FluentValidation;
using MediatR;
using System;

namespace Offers.API.Application.Commands.CreatePredefinedDeliveryMethod
{
    public class CreatePredefinedDeliveryMethodCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal? Price { get; init; }
    }

    public class CreatePredefinedDeliveryMethodCommandValidator : AbstractValidator<CreatePredefinedDeliveryMethodCommand>
    {
        public CreatePredefinedDeliveryMethodCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty();

            RuleFor(x => x.Price)
                .Must(x => x == null || x >= 0)
                .WithMessage("Must be null or >= 0");
        }
    }
}
