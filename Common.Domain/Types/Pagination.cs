using System;
using System.Collections.Generic;

namespace Common.Domain.Types
{
    public class Pagination<T>
    {
        public PageCriteria PageCriteria { get; }
        public IList<T> Items { get; }
        public int TotalPages { get; }

        public Pagination(PageCriteria pageCriteria, IList<T> items, int totalPages)
        {
            PageCriteria = pageCriteria ?? throw new ArgumentNullException(nameof(pageCriteria));
            Items = items ?? throw new ArgumentNullException(nameof(items));
            
            if (totalPages < 0) throw new ArgumentException($"'{nameof(totalPages)}' must be >= 0");
            TotalPages = totalPages;
        }
    }
}
