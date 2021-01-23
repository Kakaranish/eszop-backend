using Common.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class PaginationExtensions
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

        public static Pagination<TDestination> Transform<TSource, TDestination>(
            this Pagination<TSource> pagination, Func<IList<TSource>, IEnumerable<TDestination>> transformFunc)
        {
            if (pagination == null) throw new ArgumentNullException(nameof(pagination));
            if (transformFunc == null) throw new ArgumentNullException(nameof(transformFunc));

            var transformedItems = transformFunc(pagination.Items).ToList();
            return new Pagination<TDestination>(pagination.PageDetails, transformedItems, pagination.TotalPages);
        }
    }
}
