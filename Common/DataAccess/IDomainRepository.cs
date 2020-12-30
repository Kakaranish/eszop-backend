using Common.Domain;

namespace Common.DataAccess
{
    public interface IDomainRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
