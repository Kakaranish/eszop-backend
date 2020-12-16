namespace Common.Authentication
{
    public interface IAccessTokenDecoder
    {
        TokenPayload Decode(string accessToken);
    }
}
