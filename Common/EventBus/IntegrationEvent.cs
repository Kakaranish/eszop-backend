using System;
using RawRabbit.Context;

namespace Common.Utilities.EventBus
{
    public abstract class IntegrationEvent : IMessageContext
    {
        public Guid Id { get; }
        public DateTime CreatedAt { get; }

        protected IntegrationEvent()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
        }

        public Guid GlobalRequestId { get; set; }
    }
}
