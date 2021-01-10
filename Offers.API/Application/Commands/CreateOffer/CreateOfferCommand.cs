using Common.Extensions;
using Common.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Domain.Validators;
using System;
using System.Collections.Generic;

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
                .SetValidator(new OfferNameValidator());

            RuleFor(x => x.Price)
                .SetValidator(new OfferPriceValidator());

            RuleFor(x => x.Description)
                .SetValidator(new OfferDescriptionValidator());

            RuleFor(x => x.TotalStock)
                .SetValidator(new TotalStockValidator());

            RuleFor(x => x.CategoryId)
                .IsGuid();
        }
    }
}