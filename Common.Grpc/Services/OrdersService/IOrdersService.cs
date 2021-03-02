using Common.Grpc.Services.OrdersService.CreateOrder;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Grpc.Services.OrdersService
{
    [ServiceContract]
    public interface IOrdersService
    {
        [OperationContract]
        Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);
    }
}
