using MediatR;
using Offers.API.Domain;
using System.Collections.Generic;

namespace Offers.API.Application.Queries.GetCategories
{
    public class GetCategoriesQuery : IRequest<IList<Category>>
    {
    }
}
