using DeliveryApp.Core.Domain.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.Ports
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order Add(Order order);
        void Update(Order order);
        Task<Order> GetAsync(Guid orderId); 
        IEnumerable<Order> GetAllCreated(); 
        IEnumerable<Order> GetAllAssigned();
    }
}
