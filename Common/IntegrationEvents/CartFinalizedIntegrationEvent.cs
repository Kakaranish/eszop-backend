using Common.IntegrationEvents.Dto;
using System;
using System.Collections.Generic;
using Common.EventBus;

namespace Common.IntegrationEvents
{
    public class CartFinalizedIntegrationEvent : IntegrationEvent
    {
        public Guid UserId { get; init; }
        public IList<CartItemDto> CartItems { get; init; }
    }
}
