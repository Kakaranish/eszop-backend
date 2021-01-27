using Common.Extensions;
using Common.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Http;
using Offers.API.Domain.Validators;
using System.Collections.Generic;

namespace Offers.API.Application.Commands.UpdateOfferDraft
{
    public class UpdateOfferDraftCommand : IRequest
    {
        public string OfferId { get; init; }
        public string Name { get; init; }
        public string Description { get; init; }
        public decimal? Price { get; init; }
        public int? AvailableStock { get; init; }
        public IList<IFormFile> Images { get; init; }
        public string ImagesMetadata { get; init; }
        public string KeyValueInfos { get; init; }
    }

    public class UpdateOfferDraftCommandValidator : AbstractValidator<UpdateOfferDraftCommand>
    {
        public UpdateOfferDraftCommandValidator()
        {
            RuleFor(x => x.OfferId)
                .IsNotEmptyGuid();

            RuleFor(x => x.Name)
                .SetValidator(new OfferNameValidator())
                .When(x => x.Name is not null);

            RuleFor(x => x.Description)
                .SetValidator(new OfferDescriptionValidator())
                .When(x => x.Description is not null);

            RuleFor(x => x.Price)
                .Must(price =>
                {
                    if (price == null) return true;
                    var validator = new OfferPriceValidator();
                    return validator.Validate(price.Value).IsValid;
                })
                .When(x => x.Price is not null);

            RuleFor(x => x.AvailableStock)
                .GreaterThanOrEqualTo(0)
                .When(x => x.AvailableStock is not null);

            RuleFor(x => x.ImagesMetadata)
                .NotEmpty();

            RuleFor(x => x.KeyValueInfos)
                .Must(x => x == null || !string.IsNullOrWhiteSpace(x))
                .WithMessage("Must be null or not empty string");
        }
    }
}
