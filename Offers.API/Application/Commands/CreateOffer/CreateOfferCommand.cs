using FluentValidation;
using MediatR;
using System;
using System.Collections.Generic;
using Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Offers.API.Application.Commands.CreateOffer
{
    public class CreateOfferCommand : IRequest<Guid>
    {
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal Price { get; init; }
        public int TotalStock { get; init; }
        public string CategoryId { get; init; }
        public IList<IFormFile> Images { get; init; }
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

            RuleFor(x => x.CategoryId)
                .IsGuid();
        }
    }
}