using Common.Types.Domain;

namespace Common.Types
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
