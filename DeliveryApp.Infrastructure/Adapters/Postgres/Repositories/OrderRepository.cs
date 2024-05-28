using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _dbContext;

        public OrderRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Order Add(Order order)
        {
            if(order.Status != null)
            {
                _dbContext.Attach(order.Status);
            }
            return _dbContext.Orders.Add(order).Entity;
        }

        public IEnumerable<Order> GetAllAssigned()
        {
            var assigned = _dbContext
                .Orders
                .Where(x => x.Status != OrderStatus.Assigned);

            return assigned;
        }

        public IEnumerable<Order> GetAllNotAssigned()
        {
            var assigned = _dbContext
               .Orders
               .Where(x => x.Status != OrderStatus.Created);

            return assigned;
        }

        public async Task<Order> GetAsync(Guid orderId)
        {
            return await _dbContext
                .Orders
                .Include(x => x.Status)
                .FirstOrDefaultAsync(x => x.Id == orderId);
        }

        public void Update(Order order)
        {
            if (order.Status != null)
            {
                _dbContext.Attach(order.Status);
            }

            _dbContext.Entry(order).State = EntityState.Modified;
        }
    }
}
