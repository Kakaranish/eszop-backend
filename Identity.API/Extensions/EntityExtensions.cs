using Identity.API.Application.Dto;
using Identity.API.Domain;

namespace Identity.API.Extensions
{
    public static class EntityExtensions
    {
        public static ProfileInfoDto ToDto(this ProfileInfo profileInfo)
        {
            if (profileInfo == null) return null;

            return new ProfileInfoDto
            {
                UserId = profileInfo.UserId,
                FirstName = profileInfo.FirstName,
                LastName = profileInfo.LastName,
                PhoneNumber = profileInfo.PhoneNumber,
                DateOfBirth = profileInfo.DateOfBirth
            };
        }

        public static DeliveryAddressDto ToDto(this DeliveryAddress deliveryAddress)
        {
            if (deliveryAddress == null) return null;

            return new DeliveryAddressDto
            {
                Id = deliveryAddress.Id,
                FirstName = deliveryAddress.FirstName,
                LastName = deliveryAddress.LastName,
                PhoneNumber = deliveryAddress.PhoneNumber,
                Country = deliveryAddress.Country,
                City = deliveryAddress.City,
                ZipCode = deliveryAddress.ZipCode,
                Street = deliveryAddress.Street,
            };
        }
    }
}
