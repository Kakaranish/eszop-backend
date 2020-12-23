using Common.Types.Domain;

namespace Common.Types
{
    public interface IDomainRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
