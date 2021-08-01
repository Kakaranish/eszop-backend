using System;

namespace Carts.Domain.Exceptions
{
    public class CartsDomainException : Exception
    {
        public CartsDomainException(string? message) : base(message)
        {
        }
    }
}
