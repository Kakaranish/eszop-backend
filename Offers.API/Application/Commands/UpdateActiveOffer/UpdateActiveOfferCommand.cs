using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Common.Extensions;
using Common.Validators;
using Offers.API.Domain.Validators;

namespace Offers.API.Application.Commands.UpdateActiveOffer
{
    public class UpdateActiveOfferCommand : IRequest
    {
        public string OfferId { get; set; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal? Price { get; init; }
        public int? AvailableStock { get; init; }
        public string CategoryId { get; init; }
        public IList<IFormFile> Images { get; init; }
        public string ImagesMetadata { get; init; }
        public string KeyValueInfos { get; init; }
        public string DeliveryMethods { get; init; }
    }

    public class UpdateActiveOfferCommandValidator : AbstractValidator<UpdateActiveOfferCommand>
    {
        public UpdateActiveOfferCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .SetValidator(new OfferNameValidator())
                .When(x => x.Name != null);

            RuleFor(x => x.Description)
                .SetValidator(new OfferDescriptionValidator())
                .When(x => x.Description != null);

            RuleFor(x => x.Price)
                .Must(price =>
                {
                    var validator = new OfferPriceValidator();
                    return validator.Validate(price.Value).IsValid;
                })
                .When(x => x.Price != null);

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(1)
                .When(x => x.AvailableStock != null);

            RuleFor(x => x.CategoryId)
                .IsNotEmptyGuid()
                .When(x => x.CategoryId != null);

            RuleFor(x => x.ImagesMetadata)
                .NotEmpty();

            RuleFor(x => x.KeyValueInfos)
                .Must(x => x == null || !string.IsNullOrWhiteSpace(x))
                .WithMessage("Must be null or not empty string");

            RuleFor(x => x.DeliveryMethods)
                .NotEmpty();
        }
    }
}
