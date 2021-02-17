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
    }
}
