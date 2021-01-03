using Common.Domain;
using Common.Types;
using Common.Validators;
using FluentValidation;
using Offers.API.Application.DomainEvents.AvailableStockChanged;
using Offers.API.Application.DomainEvents.OfferBecameUnavailable;
using System;

namespace Offers.API.Domain
{
    public class Offer : EntityBase, IAggregateRoot, ITimeStamped, IRemovable
    {
        public Guid OwnerId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? UserEndedAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public DateTime? RemovedAt { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int AvailableStock { get; private set; }
        public int TotalStock { get; private set; }
        public Category Category { get; private set; }

        private Offer()
        {
        }

        public Offer(Guid ownerId, string name, string description, decimal price, int totalStock, Category category)
        {
            SetOwnerId(ownerId);
            SetName(name);
            SetDescription(description);
            SetPrice(price);
            SetTotalStock(totalStock);
            SetCategory(category);

            CreatedAt = DateTime.UtcNow;
            EndsAt = CreatedAt.AddDays(14);
        }

        private void SetOwnerId(Guid ownerId)
        {
            ValidateEditable();

            if (OwnerId == ownerId) return;
            ValidateOwnerId(ownerId);

            OwnerId = ownerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            ValidateEditable();

            if (Name == name) return;
            ValidateName(name);

            Name = name;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDescription(string description)
        {
            ValidateEditable();

            if (Description == description) return;
            ValidateDescription(description);

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrice(decimal price)
        {
            ValidateEditable();

            if (Price == price) return;
            ValidatePrice(price);

            Price = price;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetTotalStock(int totalStock)
        {
            ValidateEditable();

            if (TotalStock == totalStock) return;
            ValidateTotalStock(totalStock);

            TotalStock = totalStock;
            AvailableStock = totalStock;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetCategory(Category category)
        {
            ValidateEditable();

            ValidateCategory(category);
            if (category == Category) return;

            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        public void DecreaseAvailableStock(int toDecrease)
        {
            ValidateEditable();
            ValidateDecreaseAvailableStock(toDecrease);

            var domainEvent = new AvailableStockChangedDomainEvent
            {
                OfferId = Id,
                AvailableStock = new ChangeState<int?>(AvailableStock, AvailableStock - toDecrease)
            };
            AddDomainEvent(domainEvent);

            AvailableStock -= toDecrease;
        }

        public void EndOffer()
        {
            ValidateEndOffer();
            UserEndedAt = DateTime.UtcNow;
            UpdatedAt = UserEndedAt.Value;

            var domainEvent = new OfferBecameUnavailableDomainEvent { OfferId = Id };
            AddDomainEvent(domainEvent);
        }

        public void MarkAsRemoved()
        {
            ValidateMarkAsRemoved();

            RemovedAt = DateTime.UtcNow;
            UpdatedAt = RemovedAt.Value;

            var domainEvent = new OfferBecameUnavailableDomainEvent { OfferId = Id };
            AddDomainEvent(domainEvent);
        }

        #region Validation

        private static void ValidateOwnerId(Guid ownerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(ownerId);
            if (!result.IsValid) throw new OffersDomainException($"'{nameof(ownerId)}' is invalid id");
        }

        private static void ValidateName(string name)
        {
            var validator = new OfferNameValidator();
            var result = validator.Validate(name);
            if (!result.IsValid) throw new OffersDomainException($"'{nameof(name)}' is invalid name");
        }

        private static void ValidateDescription(string description)
        {
            var validator = new InlineValidator<string>();
            validator.RuleFor(x => x)
                .NotNull()
                .NotEmpty()
                .MinimumLength(5);

            var result = validator.Validate(description);
            if (!result.IsValid) throw new OffersDomainException($"'{nameof(description)}' is invalid description");
        }

        private static void ValidatePrice(decimal price)
        {
            var validator = new PriceValidator();
            var result = validator.Validate(price);
            if (!result.IsValid) throw new OffersDomainException($"'{nameof(price)}' is invalid price");
        }

        private static void ValidateTotalStock(int totalStock)
        {
            if (totalStock <= 0) throw new OffersDomainException($"'{nameof(totalStock)}' must be > 0");
        }

        private void ValidateDecreaseAvailableStock(int toDecrease)
        {
            if (toDecrease <= 0)
            {
                throw new OffersDomainException($"{nameof(toDecrease)} must be > 0");
            }
            if (AvailableStock < toDecrease)
            {
                throw new OffersDomainException($"{nameof(toDecrease)} cannot be greater than AvailableStock");
            }
        }

        private static void ValidateCategory(Category category)
        {
            if (category == null) throw new OffersDomainException($"'{nameof(category)}' cannot be null");
        }

        private void ValidateEndOffer()
        {
            if (UserEndedAt != null) throw new OffersDomainException("Offer is already ended");
        }

        private void ValidateEditable()
        {
            if (UserEndedAt != null || RemovedAt != null) throw new OffersDomainException("Offer is not editable");
        }

        private void ValidateMarkAsRemoved()
        {
            if (RemovedAt != null) throw new OffersDomainException("Offer is already removed");
        }

        #endregion
    }
}
