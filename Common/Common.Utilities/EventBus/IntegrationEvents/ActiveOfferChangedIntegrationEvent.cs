﻿using System;
using Common.Domain.Types;

namespace Common.Utilities.EventBus.IntegrationEvents
{
    public class ActiveOfferChangedIntegrationEvent : IntegrationEvent
    {
        public Guid OfferId { get; init; }
        public ChangeState<string> NameChange { get; init; }
        public ChangeState<decimal> PriceChange { get; init; }
        public ChangeState<int> AvailableStockChange { get; init; }
        public ChangeState<string> MainImageUriChange { get; init; }
    }
}
