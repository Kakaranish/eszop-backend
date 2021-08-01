using MediatR;
using System.Collections.Generic;
using Offers.Infrastructure.Dto;

namespace Offers.API.Application.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<IList<CategoryDto>>
    {
    }
}
