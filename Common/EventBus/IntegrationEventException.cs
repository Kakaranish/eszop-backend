using System;

namespace Common.EventBus
{
    public class IntegrationEventException : Exception
    {
        public IntegrationEventException(string? message) : base(message)
        {
        }
    }
}
