﻿using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Carts.API.DataAccess.Repositories;
using Carts.API.Domain;
using Common.Extensions;
using Microsoft.AspNetCore.Http;

namespace Carts.API.Application.Commands.UpdateCartItemQuantity
{
    public class UpdateCartItemQuantityCommandHandler : IRequestHandler<UpdateCartItemQuantityCommand>
    {
        private readonly ICartItemRepository _cartItemRepository;
        private readonly HttpContext _httpContext;

        public UpdateCartItemQuantityCommandHandler(ICartItemRepository cartItemRepository, IHttpContextAccessor httpContextAccessor)
        {
            _cartItemRepository = cartItemRepository ?? throw new ArgumentNullException(nameof(cartItemRepository));
            _httpContext = httpContextAccessor.HttpContext ??
                           throw new ArgumentNullException(nameof(httpContextAccessor.HttpContext));
        }

        public async Task<Unit> Handle(UpdateCartItemQuantityCommand request, CancellationToken cancellationToken)
        {
            var userId =_httpContext.User.Claims.ToTokenPayload().UserClaims.Id;
            var cartItemId = Guid.Parse(request.CartItemId);
            var cartItem = await _cartItemRepository.GetByIdAsync(cartItemId);

            if (cartItem is null || userId != cartItem.SellerId)
            {
                throw new CartsDomainException($"Cart item {cartItemId} not found");
            }

            if (cartItem.Quantity == request.Quantity) return await Unit.Task;
            
            cartItem.SetQuantity(request.Quantity);
            _cartItemRepository.Update(cartItem);
            await _cartItemRepository.UnitOfWork.SaveChangesAndDispatchDomainEventsAsync(cancellationToken);

            return await Unit.Task;
        }
    }
}