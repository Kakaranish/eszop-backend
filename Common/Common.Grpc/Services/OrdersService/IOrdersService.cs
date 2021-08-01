using Common.Grpc.Services.OrdersService.CreateOrder;
using Common.Grpc.Services.OrdersService.GetOfferHasOrders;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Grpc.Services.OrdersService
{
    [ServiceContract]
    public interface IOrdersService
    {
        [OperationContract]
        Task<CreateOrderResponse> CreateOrder(CreateOrderRequest request);

        [OperationContract]
        Task<GetOfferHasOrdersResponse> GetOfferHasOrders(GetOfferHasOrdersRequest request);
    }
}
