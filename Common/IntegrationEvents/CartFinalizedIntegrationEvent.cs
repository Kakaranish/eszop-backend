using Common.IntegrationEvents.Dto;
using Common.ServiceBus;
using System;
using System.Collections.Generic;

namespace Common.IntegrationEvents
{
    public class CartFinalizedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public IList<CartItemDto> CartItems { get; init; }
    }
}
