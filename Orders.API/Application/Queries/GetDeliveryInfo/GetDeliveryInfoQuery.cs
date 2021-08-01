using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries.GetDeliveryInfo
{
    public class GetDeliveryInfoQuery : IRequest<DeliveryInfoDto>
    {
        public string OrderId { get; init; }
    }

    public class GetDeliveryInfoQueryValidator : AbstractValidator<GetDeliveryInfoQuery>
    {
        public GetDeliveryInfoQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
