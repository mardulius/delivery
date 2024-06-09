using DeliveryApp.Core.Ports;
using GeoApp.Api;
using Grpc.Core;
using Grpc.Net.Client;
using Grpc.Net.Client.Configuration;


namespace DeliveryApp.Infrastructure.Adapters.Grpc.GeoService
{
    public class GeoClient : IGeoClient
    {

        private readonly string _serverUrl;
        private readonly SocketsHttpHandler _handler;
        private readonly MethodConfig _methodConfig;
        public GeoClient(string serverUrl)
        {
            _serverUrl = serverUrl;

            _handler = new SocketsHttpHandler
            {
                PooledConnectionIdleTimeout = Timeout.InfiniteTimeSpan,
                KeepAlivePingDelay = TimeSpan.FromSeconds(60),
                KeepAlivePingTimeout = TimeSpan.FromSeconds(30),
                EnableMultipleHttp2Connections = true

            };

            _methodConfig = new MethodConfig
            {
                Names = { MethodName.Default },
                RetryPolicy = new RetryPolicy
                {
                    MaxAttempts = 5,
                    InitialBackoff = TimeSpan.FromSeconds(1),
                    MaxBackoff = TimeSpan.FromSeconds(5),
                    BackoffMultiplier = 1.5,
                    RetryableStatusCodes = { StatusCode.Unavailable }
                }
            };           
        }

        public async Task<Core.Domain.SharedKernel.Location> GetGeolocationAsync(string address, CancellationToken cancellationToken)
        {
            using var channel = GrpcChannel.ForAddress(_serverUrl, new GrpcChannelOptions()
            {
                HttpHandler = _handler,
                ServiceConfig = new ServiceConfig { MethodConfigs = {_methodConfig } }
            });

            var client = new Geo.GeoClient(channel);
            var request = new GetGeolocationRequest{ Address = address};

            var result = await client.GetGeolocationAsync(request, null, DateTime.UtcNow.AddSeconds(2), cancellationToken);

            var locationCreateResult = Core.Domain.SharedKernel.Location.Create(result.Location.X, result.Location.Y);
            if(locationCreateResult.IsFailure) throw new Exception(locationCreateResult.Error.Message);

            var location = locationCreateResult.Value;

            return location;
        }
    }
}
