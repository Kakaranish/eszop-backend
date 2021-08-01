namespace Common.Utilities.Authentication
{
    public interface IAccessTokenDecoder
    {
        TokenPayload Decode(string accessToken);
    }
}
