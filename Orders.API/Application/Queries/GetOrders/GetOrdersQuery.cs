using Common.Types;
using Common.Validators;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries.GetOrders
{
    public class GetOrdersQuery : IRequest<Pagination<OrderPreviewDto>>
    {
        public BasicPaginationFilter Filter { get; init; }
    }

    public class GetOrderQueryValidator : AbstractValidator<GetOrdersQuery>
    {
        public GetOrderQueryValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new BasicPaginationFilterValidator());
        }
    }
}
