using Common.Grpc.Services.OffersService;

namespace Carts.API.Grpc
{
    public interface IOffersServiceClientFactory
    {
        IOffersService Create();
    }
}
