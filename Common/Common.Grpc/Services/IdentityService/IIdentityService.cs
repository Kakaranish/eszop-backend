using Common.Grpc.Services.IdentityService.Requests.GetBankAccountNumber;
using System.ServiceModel;
using System.Threading.Tasks;

namespace Common.Grpc.Services.IdentityService
{
    [ServiceContract]
    public interface IIdentityService
    {
        [OperationContract]
        public Task<GetBankAccountNumberResponse> GetBankAccount(GetBankAccountNumberRequest request);
    }
}
