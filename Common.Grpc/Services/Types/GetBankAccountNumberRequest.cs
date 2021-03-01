using System;
using System.Runtime.Serialization;

namespace Common.Grpc.Services.Types
{
    [DataContract]
    public class GetBankAccountNumberRequest
    {
        [DataMember(Order = 1)]
        public Guid OfferId { get; set; }
    }
}
