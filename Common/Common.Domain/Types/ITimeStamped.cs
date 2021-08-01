using System;

namespace Common.Domain.Types
{
    public interface ITimeStamped
    {
        public DateTime CreatedAt { get; }
        public DateTime UpdatedAt { get; }
    }
}
