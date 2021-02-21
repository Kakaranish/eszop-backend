using System;

namespace Orders.API.Application.Dto
{
    public class DeliveryAddressDto
    {
        public Guid Id { get; set; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string PhoneNumber { get; init; }
        public string Country { get; init; }
        public string City { get; init; }
        public string ZipCode { get; init; }
        public string Street { get; init; }
    }
}
