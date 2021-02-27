using Common.Extensions;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries.GetOrderSummary
{
    public class GetOrderSummaryQuery : IRequest<OrderDto>
    {
        public string OrderId { get; init; }
    }

    public class GetOrderSummaryQueryValidator : AbstractValidator<GetOrderSummaryQuery>
    {
        public GetOrderSummaryQueryValidator()
        {
            RuleFor(x => x.OrderId)
                .IsNotEmptyGuid();
        }
    }
}
