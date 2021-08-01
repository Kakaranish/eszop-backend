using System;

namespace Common.Domain.Types
{
    public interface IRemovable
    {
        DateTime? RemovedAt { get; }
        void MarkAsRemoved();
    }
}
