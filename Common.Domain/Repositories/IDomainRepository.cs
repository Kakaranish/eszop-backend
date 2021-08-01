namespace Common.Domain.Repositories
{
    public interface IDomainRepository<T> : IRepository<T> where T : IAggregateRoot
    {
        public IUnitOfWork UnitOfWork { get; }
    }
}
