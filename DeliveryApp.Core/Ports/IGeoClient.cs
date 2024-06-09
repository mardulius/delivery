using DeliveryApp.Core.Domain.SharedKernel;

namespace DeliveryApp.Core.Ports
{
    public interface IGeoClient
    {
        Task<Location> GetGeolocationAsync(string address, CancellationToken cancellationToken);
    }
}
