namespace Common.Authentication
{
    public class JwtConfig
    {
        public string AccessTokenSecretKey { get; set; }
        public string RefreshTokenSecretKey { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int AccessTokenExpirationInMinutes { get; set; } = 15;
        public int RefreshTokenExpirationInDays { get; set; } = 365;
    }
}
