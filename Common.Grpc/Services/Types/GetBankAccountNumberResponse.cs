using System.Runtime.Serialization;

namespace Common.Grpc.Services.Types
{
    [DataContract]
    public class GetBankAccountNumberResponse
    {
        [DataMember(Order = 1)]
        public string BankAccountNumber { get; set; }
    }
}
