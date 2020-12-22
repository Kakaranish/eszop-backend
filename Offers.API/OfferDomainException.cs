using System;

namespace Offers.API
{
    public class OfferDomainException : Exception
    {
        public OfferDomainException(string? message) : base(message)
        {
        }
    }
}
