using System;

namespace Common.Authentication
{
    public class UserClaims
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
