namespace Common.Grpc
{
    public interface IGrpcServiceClientFactory<out TService>
    {
        TService Create(string serviceUri);
    }
}
