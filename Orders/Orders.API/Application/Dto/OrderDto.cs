﻿using System;
using System.Collections.Generic;
using Common.Dto;

namespace Orders.API.Application.Dto
{
    public class OrderDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public Guid BuyerId { get; init; }
        public Guid SellerId { get; init; }
        public string OrderState { get; init; }
        public decimal TotalPrice { get; init; }
        public List<OrderItemDto> OrderItems { get; init; }
        public DeliveryAddressDto DeliveryAddress { get; init; }
        public DeliveryMethodDto DeliveryMethod { get; init; }
    }
}
