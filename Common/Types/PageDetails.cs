using System;

namespace Common.Types
{
    public class PageDetails
    {
        public int Index { get; }
        public int Size { get; }

        public PageDetails(int index, int size)
        {
            if (index <= 0) throw new ArgumentException($"'{nameof(index)}' must be > 0");
            if (size <= 0) throw new ArgumentException($"'{nameof(size)}' must be > 0");

            Index = index;
            Size = size;
        }
    }
}
