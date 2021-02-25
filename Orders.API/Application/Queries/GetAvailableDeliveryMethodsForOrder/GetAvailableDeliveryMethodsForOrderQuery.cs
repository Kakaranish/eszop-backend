using Common.Dto;
using Common.Extensions;
using FluentValidation;
using MediatR;
using System.Collections.Generic;

namespace Orders.API.Application.Queries.GetAvailableDeliveryMethodsForOrder
{
    public class GetAvailableDeliveryMethodsForOrderQuery : IRequest<IList<DeliveryMethodDto>>
    {
        public string OrderId { get; init; }
    }

    public class GetAvailableDeliveryMethodsForOrderQueryValidator
        : AbstractValidator<GetAvailableDeliveryMethodsForOrderQuery>
    {
        public GetAvailableDeliveryMethodsForOrderQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
