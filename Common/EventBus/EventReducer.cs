using Common.Domain;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;

namespace Common.EventBus
{
    public class EventReducer : IEventReducer
    {
        private readonly IServiceProvider _serviceProvider;

        public EventReducer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public IEnumerable<IDomainEvent> Reduce<T>(T entity) where T : EntityBase
        {
            var reducer = _serviceProvider.GetService<IEntityEventReducer<T>>();
            return reducer != null
                ? reducer.Reduce(entity) 
                : entity.DomainEvents;
        }
    }
}
