using System;
using System.Collections.Generic;

namespace Common.Types
{
    public class Pagination<T>
    {
        public PageDetails PageDetails { get; }
        public IList<T> Items { get; }
        public int TotalPages { get; }

        public Pagination(PageDetails pageDetails, IList<T> items, int totalPages)
        {
            PageDetails = pageDetails ?? throw new ArgumentNullException(nameof(pageDetails));
            Items = items ?? throw new ArgumentNullException(nameof(items));
            
            if (totalPages < 0) throw new ArgumentException($"'{nameof(totalPages)}' must be >= 0");
            TotalPages = totalPages;
        }
    }
}
