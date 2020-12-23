using System;

namespace Common.Types
{
    public interface ITimeStamped
    {
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
    }
}
