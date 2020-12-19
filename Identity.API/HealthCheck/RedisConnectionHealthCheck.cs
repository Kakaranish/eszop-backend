using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Identity.API.HealthCheck
{
    public class RedisConnectionHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public RedisConnectionHealthCheck(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new CancellationToken())
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);

            return redis.IsConnected
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Redis connection failed");
        }
    }
}
