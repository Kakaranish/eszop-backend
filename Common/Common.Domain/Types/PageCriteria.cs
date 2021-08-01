using System;

namespace Common.Domain.Types
{
    public class PageCriteria
    {
        public int Index { get; }
        public int Size { get; }

        public PageCriteria(int index, int size)
        {
            if (index <= 0) throw new ArgumentException($"'{nameof(index)}' must be > 0");
            if (size <= 0) throw new ArgumentException($"'{nameof(size)}' must be > 0");

            Index = index;
            Size = size;
        }
    }
}
