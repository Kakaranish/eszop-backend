using Common.Domain;
using Common.Domain.Types;
using FluentValidation;
using Offers.Domain.Exceptions;
using System;

namespace Offers.Domain.Aggregates.CategoryAggregate
{
    public class Category : EntityBase, IAggregateRoot, ITimeStamped
    {
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public string Name { get; private set; }

        public Category(string name)
        {
            SetName(name);

            CreatedAt = DateTime.UtcNow;
            UpdatedAt = CreatedAt;
        }

        public void SetName(string name)
        {
            ValidateName(name);

            Name = name;
            UpdatedAt = CreatedAt;
        }

        private static void ValidateName(string name)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(3);

            var result = validator.Validate(name);
            if (!result.IsValid) throw new OffersDomainException($"'{nameof(name)}' has invalid value");
        }
    }
}
