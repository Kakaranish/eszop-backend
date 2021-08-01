using Common.Domain.Types;
using Common.Domain.Validators;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries.GetOrdersAsBuyer
{
    public class GetOrdersAsBuyerQuery : IRequest<Pagination<OrderPreviewDto>>
    {
        public BasicPaginationFilter Filter { get; init; }
    }

    public class GetOrdersAsBuyerQueryValidator : AbstractValidator<GetOrdersAsBuyerQuery>
    {
        public GetOrdersAsBuyerQueryValidator()
        {
            RuleFor(x => x.Filter)
                .SetValidator(new BasicPaginationFilterValidator());
        }
    }
}
