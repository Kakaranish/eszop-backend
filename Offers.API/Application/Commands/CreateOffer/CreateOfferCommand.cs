using FluentValidation;
using MediatR;
using System;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int TotalStock { get; init; }
    }

    public class CreateOfferCommandValidator : AbstractValidator<CreateOfferCommand>
    {
        public CreateOfferCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.Price)
                .GreaterThan(0);

            RuleFor(x => x.Description)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            RuleFor(x => x.TotalStock)
                .GreaterThanOrEqualTo(1);
        }
    }
}