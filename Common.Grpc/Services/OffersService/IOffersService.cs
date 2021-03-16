using Common.Grpc.Services.OffersService.Requests.GetDeliveryMethodsForOffers;
using Common.Grpc.Services.OffersService.Requests.GetOfferBasicInfo;
using Common.Grpc.Services.OffersService.Requests.GetOffersAvailability;
using ProtoBuf.Grpc;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Grpc.Services.OffersService
{
    [ServiceContract]
    public interface IOffersService
    {
        [OperationContract]
        Task<GetDeliveryMethodsForOffersResponse> GetDeliveryMethodsForOffers(
            GetDeliveryMethodsForOffersRequest request, CallContext context = default);

        [OperationContract]
        Task<GetOfferBasicInfoResponse> GetOfferBasicInfo(
            GetOfferBasicInfoRequest request, CallContext context = default);

        [OperationContract]
        Task<GetOffersAvailabilityResponse> GetOffersAvailability(
            GetOffersAvailabilityRequest request, CallContext context = default);
    }
}
