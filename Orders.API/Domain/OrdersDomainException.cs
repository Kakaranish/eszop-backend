using System;

namespace Orders.API.Domain
{
    public class OrdersDomainException : Exception
    {
        public OrdersDomainException(string? message) : base(message)
        {
        }
    }
}
