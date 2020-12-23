using Common.Types.Domain;
using FluentValidation;
using System;

namespace Offers.API.Domain
{
    public class Offer : EntityBase, IAggregateRoot
    {
        public Guid OwnerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }

        public Offer(Guid ownerId, string name, string description, decimal price)
        {
            ValidateOwnerId(ownerId);
            ValidateName(name);
            ValidateDescription(description);
            ValidatePrice(price);
            
            OwnerId = ownerId;
            Name = name;
            Description = description;
            Price = price;
            
            CreatedAt = DateTime.UtcNow;
            EndsAt = CreatedAt.AddDays(14);
        }

        public void ChangePrice(decimal price)
        {
            ValidatePrice(price);
            Price = price;

            // Trigger integration event
        }
        
        private static void ValidateOwnerId(Guid ownerId)
        {
            if (ownerId == Guid.Empty) throw new DomainException($"'{nameof(ownerId)}' is invalid id");
        }

        private static void ValidateName(string name)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            var result = validator.Validate(name);
            if (!result.IsValid) throw new DomainException($"'{nameof(name)}' is invalid name");
        }

        private static void ValidateDescription(string description)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            var result = validator.Validate(description);
            if (!result.IsValid) throw new DomainException($"'{nameof(description)}' is invalid description");
        }

        private static void ValidatePrice(decimal price)
        {
            if (price <= 0) throw new DomainException($"'{nameof(price)}' is invalid price");
        }
    }
}
