using System;

namespace Identity.API.Application.Dto
{
    public class ProfileInfoDto
    {
        public Guid UserId { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public DateTime DateOfBirth { get; init; }
        public string PhoneNumber { get; init; }
    }
}
