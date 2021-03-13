using System;

namespace NotificationService.Application.Domain
{
    public class NotificationDomainException : Exception
    {
        public NotificationDomainException(string? message) : base(message)
        {
        }
    }
}
