using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.IdentityService.Requests.GetBankAccountNumber
{
    [DataContract]
    public class GetBankAccountNumberRequest
    {
        [DataMember(Order = 1)]
        public Guid UserId { get; set; }
    }
}
