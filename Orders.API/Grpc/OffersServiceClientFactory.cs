using Common.Grpc.Services;
using Common.Types;
using Grpc.Net.Client;
using Microsoft.Extensions.Options;
using ProtoBuf.Grpc.Client;
using System;

namespace Orders.API.Grpc
{
    public class OffersServiceClientFactory : IOffersServiceClientFactory
    {
        private readonly ServicesEndpointsConfig _endpointsConfig;

        public OffersServiceClientFactory(IOptions<ServicesEndpointsConfig> options)
        {
            _endpointsConfig = options.Value ?? throw new ArgumentNullException(nameof(options.Value));
        }

        public IOffersService Create()
        {
            var uri = $"{_endpointsConfig.Offers.Grpc.Hostname}:{_endpointsConfig.Offers.Grpc.Port}";
            var channel = GrpcChannel.ForAddress(uri);
            var client = channel.CreateGrpcService<IOffersService>();

            return client;
        }
    }
}
