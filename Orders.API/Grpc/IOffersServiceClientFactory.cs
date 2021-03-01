using Common.Grpc.Services;

namespace Orders.API.Grpc
{
    public interface IOffersServiceClientFactory
    {
        IOffersService Create();
    }
}