using System;

namespace Common.Authentication
{
    public class TokenPayload
    {
        public Guid Id { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public DateTime Expires { get; set; }
        public UserClaims UserClaims { get; set; }
    }
}
