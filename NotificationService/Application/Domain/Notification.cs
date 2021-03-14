using Common.Domain;
using System;
using System.Collections.Generic;

namespace NotificationService.Application.Domain
{
    public class Notification : EntityBase, IAggregateRoot
    {
        public Guid UserId { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public string Message { get; private set; }
        public string Code { get; private set; }
        public IDictionary<string, string> Metadata { get; private set; }
        public bool IsRead { get; private set; }

        private Notification()
        {
        }

        public Notification(Guid userId, DateTime createdAt, string message)
        {
            SetUserId(userId);
            SetCreatedAt(createdAt);
            SetMessage(message);
        }

        private void SetUserId(Guid userId)
        {
            ValidateUserId(userId);

            UserId = userId;
        }

        private void SetCreatedAt(DateTime createdAt)
        {
            ValidateCreatedAt(createdAt);

            CreatedAt = createdAt;
        }

        private void SetMessage(string message)
        {
            ValidateMessage(message);

            Message = message;
        }

        public void SetCode(string code)
        {
            Code = code;
        }

        public void SetMetadata(IDictionary<string, string> details)
        {
            ValidateMetadata(details);

            Metadata = details;
        }

        public void SetIsRead(bool isRead)
        {
            IsRead = isRead;
        }

        #region Validation

        private void ValidateUserId(Guid userId)
        {
            if (userId == Guid.Empty) throw new NotificationDomainException($"{nameof(UserId)} cannot be empty guid");
        }

        private void ValidateCreatedAt(DateTime createdAt)
        {
            if (DateTime.UtcNow - createdAt > TimeSpan.FromMinutes(5))
                throw new NotificationDomainException($"{nameof(CreatedAt)} must be created max 5 minutes before NOW");
        }

        private void ValidateMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new NotificationDomainException($"{nameof(Message)} cannot be null or whitespace");
        }

        private void ValidateMetadata(IDictionary<string, string> details)
        {
            if (details == null || details.Keys.Count == 0)
                throw new NotificationDomainException($"{nameof(Metadata)} cannot be null or empty dictionary");
        }

        #endregion
    }
}
