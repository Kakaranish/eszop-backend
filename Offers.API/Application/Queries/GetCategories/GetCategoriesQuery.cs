using MediatR;
using Offers.API.Application.Dto;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<IList<CategoryDto>>
    {
    }
}
