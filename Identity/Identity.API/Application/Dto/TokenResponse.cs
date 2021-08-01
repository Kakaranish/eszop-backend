namespace Identity.API.Application.Dto
{
    public class TokenResponse
    {
        public string AccessToken { get; init; }
        public string RefreshToken { get; init; }
    }
}
