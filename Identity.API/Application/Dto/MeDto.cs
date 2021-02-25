using System;

namespace Identity.API.Application.Dto
{
    public class MeDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
    }
}
