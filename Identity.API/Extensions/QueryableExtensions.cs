using Identity.API.Application.Types;
using Identity.API.Domain;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Identity.API.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<User> ApplyFilter(this IQueryable<User> queryable, UserFilter filter)
        {
            if (queryable == null) return null;

            if (!string.IsNullOrWhiteSpace(filter.Role))
                queryable = queryable.Where(x => x.Role == Role.Parse(filter.Role));
            if (!string.IsNullOrWhiteSpace(filter.SearchPhrase))
            {
                queryable = queryable.Where(x =>
                    EF.Functions.Like(x.Id.ToString(), $"%{filter.SearchPhrase}%") ||
                    EF.Functions.Like(x.Email, $"%{filter.SearchPhrase}%")
                );
            }

            return queryable;
        }
    }
}
