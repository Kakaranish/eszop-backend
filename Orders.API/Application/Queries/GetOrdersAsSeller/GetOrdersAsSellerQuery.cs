using Common.Types;
using Common.Validators;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries.GetOrdersAsSeller
{
    public class GetOrdersAsSellerQuery : IRequest<Pagination<OrderPreviewDto>>
    {
        public BasicPaginationFilter Filter { get; init; }
    }

    public class GetOrdersAsSellerQueryValidator : AbstractValidator<GetOrdersAsSellerQuery>
    {
        public GetOrdersAsSellerQueryValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new BasicPaginationFilterValidator());
        }
    }
}
