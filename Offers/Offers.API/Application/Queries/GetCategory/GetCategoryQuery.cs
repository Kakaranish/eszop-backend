using Common.Utilities.Extensions;
using FluentValidation;
using MediatR;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetCategory
{
    public class GetCategoryQuery : IRequest<CategoryDto>
    {
        public string CategoryId { get; set; }
    }

    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.CategoryId)
                .IsNotEmptyGuid();
        }
    }
}
