using System;

namespace Offers.API.Domain
{
    public class OffersDomainException : Exception
    {
        public OffersDomainException(string? message) : base(message)
        {
        }
    }
}
