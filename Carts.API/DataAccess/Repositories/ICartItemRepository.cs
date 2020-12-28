using Carts.API.Domain;
using Common.IntegrationEvents;
using Common.Types;
using System.Threading.Tasks;

namespace Carts.API.DataAccess.Repositories
{
    public interface ICartItemRepository : IDomainRepository<CartItem>
    {
        public Task UpdateWithOfferChangedEvent(OfferChangedIntegrationEvent @event);
    }
}
