using Microsoft.EntityFrameworkCore;
using Offers.API.Application.Types;
using Offers.API.Domain;
using System.Linq;

namespace Offers.API.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<Offer> ApplyFilter(this IQueryable<Offer> queryable, OfferFilter filter)
        {
            if (queryable == null) return null;

            if (filter.FromPrice != null) queryable = queryable.Where(x => x.Price >= filter.FromPrice);
            if (filter.ToPrice != null) queryable = queryable.Where(x => x.Price <= filter.ToPrice);
            if (filter.CategoryId != null) queryable = queryable.Where(x => x.Category.Id == filter.CategoryId);

            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
                queryable = queryable.Where(x => EF.Functions.Like(x.Name, $"%{filter.SearchPhrase}%"));

            return queryable;
        }
    }
}
