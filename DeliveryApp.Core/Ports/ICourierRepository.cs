using DeliveryApp.Core.Domain.CourierAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports
{
    public interface ICourierRepository : IRepository<Courier>
    {
        Courier Add(Courier courier);
        Task<Courier> GetAsync(Guid courierId);
        void Update(Courier courier);
        IEnumerable<Courier> GetAllReady();
        IEnumerable<Courier> GetAllBusy();
    }
}
