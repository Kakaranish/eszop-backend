using System;

namespace Common.Utilities.Authentication
{
    public class UserClaims
    {
        public Guid Id { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
    }
}
