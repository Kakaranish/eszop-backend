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
            if (filter.Category != null) queryable = queryable.Where(x => x.Category.Id == filter.Category);

            return queryable;
        }
    }
}
