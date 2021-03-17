using System;

namespace Identity.API.Application.Dto
{
    public class UserPreviewDto
    {
        public Guid Id { get; init; }
        public string Email { get; init; }
        public string Role { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime UpdatedAt { get; init; }
        public DateTime? LastLogin { get; init; }
        public bool IsLocked { get; init; }
    }
}
