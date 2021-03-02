using System.Runtime.Serialization;

namespace Common.Grpc.Services.OffersService.Requests.GetBankAccountNumber
{
    [DataContract]
    public class GetBankAccountNumberResponse
    {
        [DataMember(Order = 1)]
        public string BankAccountNumber { get; set; }
    }
}
