using System;

namespace Offers.Domain.Exceptions
{
    public class OffersDomainException : Exception
    {
        public OffersDomainException(string? message) : base(message)
        {
        }
    }
}
