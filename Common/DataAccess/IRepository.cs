using Common.Domain;

namespace Common.DataAccess
{
    public interface IRepository<T> where T : IAggregateRoot
    {
    }
}
