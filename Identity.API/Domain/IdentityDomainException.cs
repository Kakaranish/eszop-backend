using System;

namespace Identity.API.Domain
{
    public class IdentityDomainException : Exception
    {
        public IdentityDomainException(string? message) : base(message)
        {
        }
    }
}
