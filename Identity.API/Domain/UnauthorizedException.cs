using System;

namespace Identity.API.Domain
{
    public class UnauthorizedException : Exception
    {
        public UnauthorizedException()
        {
        }

        public UnauthorizedException(string? message) : base(message)
        {
        }
    }
}
