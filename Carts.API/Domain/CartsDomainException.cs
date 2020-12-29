using System;

namespace Carts.API.Domain
{
    public class CartsDomainException : Exception
    {
        public CartsDomainException(string? message) : base(message)
        {
        }
    }
}
