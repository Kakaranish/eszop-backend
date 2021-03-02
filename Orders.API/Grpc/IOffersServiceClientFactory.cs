using Common.Grpc.Services.OffersService;

namespace Orders.API.Grpc
{
    public interface IOffersServiceClientFactory
    {
        IOffersService Create();
    }
}