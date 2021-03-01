using Common.Grpc.Services.Types;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Grpc.Services
{
    [ServiceContract]
    public interface IOffersService
    {
        [OperationContract]
        Task<GetBankAccountNumberResponse> GetBankAccount(
            GetBankAccountNumberRequest request, CallContext context = default);

        [OperationContract]
        Task<GetDeliveryMethodsForOffersResponse> GetDeliveryMethodsForOffers(
            GetDeliveryMethodsForOffersRequest request, CallContext context = default);
    }
}
