using System;

namespace Products.API
{
    public class OfferDomainException : Exception
    {
        public OfferDomainException(string? message) : base(message)
        {
        }
    }
}
