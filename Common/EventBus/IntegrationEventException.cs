using System;

namespace Common.Utilities.EventBus
{
    public class IntegrationEventException : Exception
    {
        public IntegrationEventException(string? message) : base(message)
        {
        }
    }
}
