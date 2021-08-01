using Identity.API.Application.Dto;
using Identity.Domain.Aggregates.SellerInfoAggregate;
using Identity.Domain.Aggregates.UserAggregate;

namespace Identity.API.Extensions
{
    public static class EntityExtensions
    {
        public static UserPreviewDto ToPreviewDto(this User user)
        {
            if (user == null) return null;

            return new UserPreviewDto
            {
                Id = user.Id,
                Role = user.Role.Name,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                Email = user.Email,
                LastLogin = user.LastLogin,
                IsLocked = user.IsLocked
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

        public static SellerInfoDto ToDto(this SellerInfo sellerInfo)
        {
            if (sellerInfo == null) return null;

            return new SellerInfoDto
            {
                Id = sellerInfo.Id,
                BankAccountNumber = sellerInfo.BankAccountNumber,
                ContactEmail = sellerInfo.ContactEmail,
                PhoneNumber = sellerInfo.PhoneNumber,
                AdditionalInfo = sellerInfo.AdditionalInfo
            };
        }
    }
}
