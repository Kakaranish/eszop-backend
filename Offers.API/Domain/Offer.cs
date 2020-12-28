using Common.Types.Domain;
using FluentValidation;
using System;
using Common.Types;
using Offers.API.Application.DomainEvents.OutOfStock;

namespace Offers.API.Domain
{
    public class Offer : EntityBase, IAggregateRoot, ITimeStamped
    {
        public Guid OwnerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int AvailableStock { get; private set; }
        public int TotalStock { get; private set; }

        public Offer(Guid ownerId, string name, string description, decimal price, int totalStock)
        {
            SetOwnerId(ownerId);
            SetName(name);
            SetDescription(description);
            SetPrice(price);
            SetTotalStock(totalStock);
            
            CreatedAt = DateTime.UtcNow;
            EndsAt = CreatedAt.AddDays(14);
        }

        private void SetOwnerId(Guid ownerId)
        {
            if (OwnerId == ownerId) return;
            ValidateOwnerId(ownerId);
            
            OwnerId = ownerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            if (Name == name) return;
            ValidateName(name);
            
            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            if (Description == description) return;
            ValidateDescription(description);
            
            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrice(decimal price)
        {
            if(Price == price) return;
            ValidatePrice(price);
            
            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }
        
        public void SetTotalStock(int totalStock)
        {
            if(TotalStock == totalStock) return;
            ValidateTotalStock(totalStock);

            TotalStock = totalStock;
            AvailableStock = totalStock;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecreaseAvailableStock(int toDecrease)
        {
            if(AvailableStock < toDecrease)
            {
                throw new DomainException($"'{nameof(toDecrease)}' cannot be less than AvailableStock");
            }

            AvailableStock -= toDecrease;
            if (AvailableStock != 0) return;

            var domainEvent = new OfferOutOfStockDomainEvent();
            AddDomainEvent(domainEvent);
        }

        #region Validation

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

        private static void ValidateTotalStock(int totalStock)
        {
            if (totalStock < 1) throw new DomainException($"'{nameof(totalStock)}' must be >= 1");
        }

        #endregion
    }
}
