using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using StackExchange.Redis;

namespace Common.HealthCheck
{
    public class RedisConnectionHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;

        public RedisConnectionHealthCheck(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context,
            CancellationToken cancellationToken = new())
        {
            var redis = await ConnectionMultiplexer.ConnectAsync(_connectionString);

            return redis.IsConnected
                ? HealthCheckResult.Healthy()
                : HealthCheckResult.Unhealthy("Redis connection failed");
        }
    }
}
