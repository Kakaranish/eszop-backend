using System.Collections.Generic;
using System.Linq;

namespace Common.Utilities.Extensions
{
    public static class CollectionExtensions
    {
        public static IEnumerable<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> source, int startIndex = 0)
        {
            return source.Select((item, index) => (item, index + startIndex));
        }
    }
}
