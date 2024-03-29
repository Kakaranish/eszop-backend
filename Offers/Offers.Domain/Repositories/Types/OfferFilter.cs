﻿using FluentValidation;
using System;

namespace Offers.Domain.Repositories.Types
{
    public class OfferFilter
    {
        public int PageSize { get; init; } = 10;
        public int PageIndex { get; init; } = 1;
        public decimal? FromPrice { get; init; }
        public decimal? ToPrice { get; init; }
        public Guid? CategoryId { get; init; }
        public string SearchPhrase { get; init; }
    }

    public class OfferFilterValidator : AbstractValidator<OfferFilter>
    {
        public OfferFilterValidator()
        {
            RuleFor(x => x.PageSize)
                .GreaterThan(0);

            RuleFor(x => x.PageIndex)
                .GreaterThan(0);

            RuleFor(x => x.CategoryId)
                .Must(x => x != Guid.Empty)
                .WithMessage("Cannot be empty guid");

            RuleFor(x => x.FromPrice)
                .GreaterThan(0)
                .When(x => x.FromPrice != null);

            RuleFor(x => x.ToPrice)
                .GreaterThan(0)
                .When(x => x.ToPrice != null);

            RuleFor(x => x.ToPrice)
                .GreaterThanOrEqualTo(x => x.FromPrice)
                .When(x => x.FromPrice != null && x.ToPrice != null);
        }
    }
}
