using Common.Types;
using Common.Validators;
using FluentValidation;
using MediatR;
using Orders.API.Application.Dto;

namespace Orders.API.Application.Queries
{
    public class GetOrdersQuery : IRequest<Pagination<OrderPreviewDto>>
    {
        public BasicPaginationFilter Filter { get; set; }
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
