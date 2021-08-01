using System;

namespace NotificationService.Domain.Exceptions
{
    public class NotificationDomainException : Exception
    {
        public NotificationDomainException(string? message) : base(message)
        {
        }
    }
}
