using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;

namespace Common.Domain.EventDispatching
{
    public class EventDispatcher : IEventDispatcher
    {
        private readonly IEventReducer _reducer;
        private readonly IMediator _mediator;

        public EventDispatcher(IEventReducer reducer, IMediator mediator)
        {
            _reducer = reducer ?? throw new ArgumentNullException(nameof(reducer));
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Dispatch(IList<EntityBase> entities)
        {
            foreach (var entity in entities)
            {
                var reducedEvents = _reducer.Reduce(entity);
                foreach (var domainEvent in reducedEvents)
                {
                    await _mediator.Publish(domainEvent);
                }
            }
        }
    }
}
