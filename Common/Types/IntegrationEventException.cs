using System;

namespace Common.Types
{
    public class IntegrationEventException : Exception
    {
        public IntegrationEventException(string? message) : base(message)
        {
        }
    }
}
