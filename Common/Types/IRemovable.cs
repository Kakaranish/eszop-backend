using System;

namespace Common.Types
{
    public interface IRemovable
    {
        DateTime? RemovedAt { get; }
        void MarkAsRemoved();
    }
}
