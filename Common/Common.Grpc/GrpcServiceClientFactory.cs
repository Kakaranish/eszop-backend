using Grpc.Net.Client;
using ProtoBuf.Grpc.Client;

namespace Common.Grpc
{
    public class GrpcServiceClientFactory<TService> : IGrpcServiceClientFactory<TService>
        where TService : class
    {
        public TService Create(string serviceUri)
        {
            var channel = GrpcChannel.ForAddress(serviceUri);
            var client = channel.CreateGrpcService<TService>();

            return client;
        }
    }
}
