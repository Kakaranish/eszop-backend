using System;

namespace Carts.API
{
    public class CartDomainException : Exception
    {
        public CartDomainException(string? message) : base(message)
        {
        }
    }
}
