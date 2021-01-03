using Common.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<Pagination<T>> PaginateAsync<T>(this IQueryable<T> queryable, PageDetails pageDetails)
        {
            var paginatedQueryable = queryable
                .Skip((pageDetails.Index - 1) * pageDetails.Size)
                .Take(pageDetails.Size);
            var items = await paginatedQueryable.ToListAsync();

            var totalCount = await queryable.CountAsync();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageDetails.Size);

            return new Pagination<T>(pageDetails, items, totalPages);
        }
    }
}
