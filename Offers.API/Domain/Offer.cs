using Common.Domain;
using Common.Types;
using Common.Validators;
using Offers.API.Application.DomainEvents.AvailableStockChanged;
using Offers.API.Application.DomainEvents.OfferBecameUnavailable;
using Offers.API.Domain.Validators;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace Offers.API.Domain
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
        public string BankAccountNumber { get; private set; }

        public virtual Category Category { get; private set; }
        public IReadOnlyCollection<DeliveryMethod> DeliveryMethods => _deliveryMethods;
        public IReadOnlyCollection<ImageInfo> Images => _images;
        public IReadOnlyCollection<KeyValueInfo> KeyValueInfos => _keyValueInfos;

        [NotMapped] public bool IsPublished => PublishedAt != null;
        [NotMapped] public bool IsDraft => PublishedAt == null;

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

        public void AddImage(ImageInfo image)
        {
            ValidateAddImage(image);
            _images ??= new List<ImageInfo>();
            _images.Add(image);
        }

        public void RemoveImage(ImageInfo imageInfo)
        {
            if (_images == null || _images.Count == 0)
                throw new OffersDomainException("Offer has no such image to remove");

            var removed = _images.Remove(imageInfo);

            if (!removed)
                throw new OffersDomainException("Offer has no such image to remove");
        }

        public void ClearImages()
        {
            _images?.Clear();
        }

        public void SetKeyValueInfos(IList<KeyValueInfo> keyValueInfos)
        {
            ValidateKeyValueInfos(keyValueInfos);

            _keyValueInfos = keyValueInfos?.ToList();
        }

        public void ClearKeyValueInfos()
        {
            _keyValueInfos = null;
        }

        public void SetBankAccountNumber(string bankAccountNumber)
        {
            ValidateBankAccountNumber(bankAccountNumber);

            BankAccountNumber = !string.IsNullOrWhiteSpace(bankAccountNumber) ? bankAccountNumber : null;
            UpdatedAt = DateTime.UtcNow;
        }

        #region Validation

        private static void ValidateOwnerId(Guid ownerId)
        {
            var validator = new IdValidator();
            var result = validator.Validate(ownerId);
            if (!result.IsValid) throw new OffersDomainException(nameof(OwnerId));
        }

        private static void ValidateName(string name)
        {
            var validator = new OfferNameValidator();
            var result = validator.Validate(name);
            if (!result.IsValid) throw new OffersDomainException(nameof(Name));
        }

        private static void ValidateDescription(string description)
        {
            var validator = new OfferDescriptionValidator();
            var result = validator.Validate(description);
            if (!result.IsValid) throw new OffersDomainException(nameof(Description));
        }

        private static void ValidatePrice(decimal price)
        {
            var validator = new OfferPriceValidator();
            var result = validator.Validate(price);
            if (!result.IsValid) throw new OffersDomainException(nameof(Price));
        }

        private static void ValidateTotalStock(int totalStock)
        {
            var validator = new TotalStockValidator();
            var result = validator.Validate(totalStock);
            if (!result.IsValid) throw new OffersDomainException(nameof(TotalStock));
        }

        private void ValidateDecreaseAvailableStock(int toDecrease)
        {
            if (toDecrease <= 0)
                throw new OffersDomainException($"{nameof(toDecrease)} must be > 0");
            if (AvailableStock < toDecrease)
                throw new OffersDomainException($"{nameof(toDecrease)} cannot be greater than {nameof(AvailableStock)}");
        }

        private static void ValidateCategory(Category category)
        {
            if (category == null) throw new OffersDomainException($"{nameof(Category)} cannot be null");
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

        private void ValidateCanBePublished()
        {
            if (IsPublished)
                throw new OffersDomainException("Offer is already published");
            if (_deliveryMethods == null || _deliveryMethods.Count == 0)
                throw new OffersDomainException("Offer has no delivery methods set");
            if (string.IsNullOrWhiteSpace(BankAccountNumber))
                throw new OffersDomainException($"Offer has no {nameof(BankAccountNumber)}");
        }

        private void ValidateAddImage(ImageInfo image)
        {
            if (image == null)
                throw new OffersDomainException($"'{nameof(image)}' cannot be null");
            if (image.IsMain && (_images?.Any(x => x.IsMain) ?? false))
                throw new OffersDomainException("There is already main image");
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

        private static void ValidateBankAccountNumber(string bankAccountNumber)
        {
            if (string.IsNullOrEmpty(bankAccountNumber)) return;

            var validator = new BankAccountNumberValidator();
            var result = validator.Validate(bankAccountNumber);
            if (!result.IsValid) throw new OffersDomainException(nameof(bankAccountNumber));
        }

        #endregion
    }
}
