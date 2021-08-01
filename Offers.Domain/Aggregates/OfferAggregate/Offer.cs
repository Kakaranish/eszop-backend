using Common.Domain;
using Common.Domain.Types;
using Common.Domain.Validators;
using Offers.Domain.Aggregates.CategoryAggregate;
using Offers.Domain.DomainEvents;
using Offers.Domain.DomainEvents.ActiveOfferChanged.PartialEvents;
using Offers.Domain.Exceptions;
using Offers.Domain.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Offers.Domain.Aggregates.OfferAggregate
{
    public class Offer : EntityBase, IAggregateRoot, ITimeStamped, IRemovable
    {
        private List<DeliveryMethod> _deliveryMethods;
        private List<ImageInfo> _images;
        private List<KeyValueInfo> _keyValueInfos;

        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }
        public DateTime? UserEndedAt { get; private set; }
        public DateTime EndsAt { get; private set; }
        public DateTime? RemovedAt { get; private set; }
        public DateTime? PublishedAt { get; set; }

        public Guid OwnerId { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public decimal Price { get; private set; }
        public int AvailableStock { get; private set; }
        public int TotalStock { get; private set; }

        public virtual Category Category { get; private set; }
        public IReadOnlyCollection<DeliveryMethod> DeliveryMethods => _deliveryMethods;
        public IReadOnlyCollection<ImageInfo> Images => _images?.OrderBy(x => x.SortId).ToList() ?? new List<ImageInfo>();
        public IReadOnlyCollection<KeyValueInfo> KeyValueInfos => _keyValueInfos;

        [NotMapped] public bool IsPublished => PublishedAt != null;
        [NotMapped] public bool IsDraft => PublishedAt == null;

        [NotMapped]
        public bool IsActive =>
            PublishedAt != null && DateTime.UtcNow < EndsAt && UserEndedAt == null && RemovedAt == null;

        protected Offer()
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
            UpdatedAt = CreatedAt;
        }

        private void SetOwnerId(Guid ownerId)
        {
            ValidateOwnerId(ownerId);
            if (OwnerId == ownerId) return;

            OwnerId = ownerId;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetName(string name)
        {
            ValidateName(name);
            if (Name == name) return;

            var previousName = Name;
            Name = name;
            UpdatedAt = DateTime.UtcNow;

            if (IsActive)
            {
                var domainEvent = new NameChangedDomainEvent
                {
                    NameChange = new ChangeState<string>(previousName, Name)
                };
                AddDomainEvent(domainEvent);
            }
        }

        public void SetDescription(string description)
        {
            ValidateDescription(description);
            if (Description == description) return;

            Description = description;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPrice(decimal price)
        {
            ValidatePrice(price);
            if (Price == price) return;

            var previousPrice = Price;
            Price = price;
            UpdatedAt = DateTime.UtcNow;

            if (IsActive)
            {
                var domainEvent = new PriceChangedDomainEvent
                {
                    PriceChange = new ChangeState<decimal>(previousPrice, price)
                };
                AddDomainEvent(domainEvent);
            }
        }

        public void SetTotalStock(int totalStock)
        {
            ValidateSetTotalStock(totalStock);
            if (TotalStock == totalStock) return;

            TotalStock = totalStock;
            AvailableStock = totalStock;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetAvailableStock(int availableStock, bool totalStockIndependent = true)
        {
            ValidateSetAvailableStock(availableStock, totalStockIndependent);
            if (availableStock == AvailableStock) return;

            var previousAvailableStock = AvailableStock;

            if (totalStockIndependent)
            {
                AvailableStock = availableStock;
            }
            else
            {
                var stockDiff = TotalStock - AvailableStock;
                AvailableStock = availableStock;
                TotalStock = availableStock + stockDiff;
            }

            UpdatedAt = DateTime.UtcNow;

            if (IsActive)
            {
                var domainEvent = new AvailableStockChangedDomainEvent
                {
                    AvailableStockChange = new ChangeState<int>(previousAvailableStock, availableStock)
                };
                AddDomainEvent(domainEvent);
            }
        }

        public void SetCategory(Category category)
        {
            ValidateCategory(category);
            if (category == Category) return;

            Category = category;
            UpdatedAt = DateTime.UtcNow;
        }

        public void EndOffer()
        {
            ValidateEndOffer();

            UserEndedAt = DateTime.UtcNow;
            UpdatedAt = UserEndedAt.Value;

            var domainEvent = new OfferBecameUnavailableDomainEvent
            {
                OfferId = Id,
                Trigger = UnavailabilityTrigger.End
            };
            AddDomainEvent(domainEvent);
        }

        public void MarkAsRemoved()
        {
            ValidateMarkAsRemoved();

            RemovedAt = DateTime.UtcNow;
            UpdatedAt = RemovedAt.Value;

            var domainEvent = new OfferBecameUnavailableDomainEvent
            {
                OfferId = Id,
                Trigger = UnavailabilityTrigger.Removal
            };
            AddDomainEvent(domainEvent);
        }

        public void SetDeliveryMethods(IList<DeliveryMethod> deliveryMethods)
        {
            ValidateDeliveryMethods(deliveryMethods);

            _deliveryMethods = deliveryMethods.ToList();
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetPublished()
        {
            ValidateCanBePublished();

            PublishedAt = DateTime.UtcNow;
            EndsAt = PublishedAt.Value.AddDays(14);
            UpdatedAt = PublishedAt.Value;
        }

        public void SetImages(IList<ImageInfo> images)
        {
            ValidateImages(images);

            _images ??= new List<ImageInfo>();
            var previousMainUri = _images.FirstOrDefault(x => x.IsMain)?.Uri;

            _images = (List<ImageInfo>)images;

            var currentMainUri = images.First(x => x.IsMain).Uri;

            if (IsActive && previousMainUri != currentMainUri)
            {
                var domainEvent = new MainImageChangedDomainEvent
                {
                    MainImageUriChange = new ChangeState<string>(previousMainUri, currentMainUri)
                };
                AddDomainEvent(domainEvent);
            }
        }

        public void SetKeyValueInfos(IList<KeyValueInfo> keyValueInfos)
        {
            ValidateKeyValueInfos(keyValueInfos);

            _keyValueInfos = keyValueInfos?.ToList();
        }

        #region Validation

        private void ValidateOwnerId(Guid ownerId)
        {
            ValidateEditable();

            var validator = new IdValidator();
            var result = validator.Validate(ownerId);
            if (!result.IsValid) throw new OffersDomainException(nameof(OwnerId));
        }

        private void ValidateName(string name)
        {
            ValidateEditable();

            var validator = new OfferNameValidator();
            var result = validator.Validate(name);
            if (!result.IsValid) throw new OffersDomainException(nameof(Name));
        }

        private void ValidateDescription(string description)
        {
            ValidateEditable();

            var validator = new OfferDescriptionValidator();
            var result = validator.Validate(description);
            if (!result.IsValid) throw new OffersDomainException(nameof(Description));
        }

        private void ValidatePrice(decimal price)
        {
            ValidateEditable();

            var validator = new OfferPriceValidator();
            var result = validator.Validate(price);
            if (!result.IsValid) throw new OffersDomainException(nameof(Price));
        }

        private void ValidateSetTotalStock(int totalStock)
        {
            if (IsPublished)
                throw new OffersDomainException($"Can't set {nameof(TotalStock)} when offer is published");

            var validator = new TotalStockValidator();
            var result = validator.Validate(totalStock);
            if (!result.IsValid) throw new OffersDomainException(nameof(TotalStock));
        }

        private void ValidateSetAvailableStock(int availableStock, bool totalStockIndependent)
        {
            ValidateEditable();

            if (availableStock < 0)
                throw new OffersDomainException($"{nameof(AvailableStock)} must be > 0");

            if (totalStockIndependent && availableStock > TotalStock)
                throw new OffersDomainException($"{nameof(AvailableStock)} cannot be greater than {nameof(TotalStock)}");
        }

        private void ValidateCategory(Category category)
        {
            ValidateEditable();
            if (category == null) throw new OffersDomainException($"{nameof(Category)} cannot be null");
        }

        private void ValidateEndOffer()
        {
            if (UserEndedAt != null || EndsAt < DateTime.UtcNow)
                throw new OffersDomainException("Offer is already ended");
        }

        private void ValidateEditable()
        {
            if (UserEndedAt != null || RemovedAt != null) throw new OffersDomainException("Offer is not editable");
        }

        private void ValidateMarkAsRemoved()
        {
            if (RemovedAt != null) throw new OffersDomainException("Offer is already removed");
        }

        private void ValidateCanBePublished()
        {
            if (IsPublished)
                throw new OffersDomainException("Offer is already published");
            if (_deliveryMethods == null || _deliveryMethods.Count == 0)
                throw new OffersDomainException("Offer has no delivery methods set");
        }

        private void ValidateImages(IList<ImageInfo> images)
        {
            ValidateEditable();

            if (images == null || !images.Any())
                throw new OffersDomainException($"{nameof(Images)} cannot be null or empty collection");

            if (images.Count(x => x.IsMain) != 1)
                throw new OffersDomainException($"{nameof(Images)} must have one and only one main image");

            if (images.Select(x => x.SortId).Distinct().Count() != images.Count)
                throw new OffersDomainException($"{nameof(Images)} sort ids must be unique");
        }

        private void ValidateKeyValueInfos(IList<KeyValueInfo> keyValueInfos)
        {
            if (keyValueInfos == null) return;
            var hasUniqueKeys = keyValueInfos.Select(x => x.Key).Distinct().Count() == keyValueInfos.Count;
            if (!hasUniqueKeys) throw new OffersDomainException($"Two {nameof(KeyValueInfo)} cannot have the same key");
        }

        private void ValidateDeliveryMethods(IList<DeliveryMethod> deliveryMethods)
        {
            if (deliveryMethods == null)
                throw new OffersDomainException($"'{nameof(deliveryMethods)}' cannot be null");

            if (deliveryMethods.Count == 0)
                throw new OffersDomainException($"{nameof(DeliveryMethods)} must contain at least one element");

            var hasUniqueNames = deliveryMethods.Select(x => x.Name).Distinct().Count() == deliveryMethods.Count;
            if (!hasUniqueNames)
                throw new OffersDomainException($"Two {nameof(DeliveryMethods)} cannot have the same key");

            if (deliveryMethods.Any(x => string.IsNullOrWhiteSpace(x.Name) || x.Price < 0))
                throw new OffersDomainException($"{nameof(DeliveryMethods)} contains at least one invalid entry");
        }

        #endregion
    }
}
