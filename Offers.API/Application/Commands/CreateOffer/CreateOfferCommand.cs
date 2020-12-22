using System;
using FluentValidation;
using MediatR;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommand : IRequest<Guid>
    {
        public string Name { get; }
        public string Description { get; }
        public decimal Price { get; }

        public CreateOfferCommand(string name, string description, decimal price)
        {
            Name = name;
            Description = description;
            Price = price;
        }
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
        }
    }
}