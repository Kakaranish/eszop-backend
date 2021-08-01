using System;

namespace Identity.API.Application.Dto
{
    public class SellerInfoDto
    {
        public Guid Id { get; init; }
        public string ContactEmail { get; init; }
        public string PhoneNumber { get; init; }
        public string BankAccountNumber { get; init; }
        public string AdditionalInfo { get; init; }
    }
}
