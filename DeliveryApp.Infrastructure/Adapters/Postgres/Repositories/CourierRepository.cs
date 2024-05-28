using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.Repositories
{
    public class CourierRepository : ICourierRepository
    {
        private readonly AppDbContext _dbContext;

        public CourierRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public Courier Add(Courier courier)
        {
            if (courier.Transport != null)
            {
                _dbContext.Attach(courier.Transport);
            }
            if (courier.Status != null)
            {
                _dbContext.Attach(courier.Status);
            }

            return _dbContext.Add(courier).Entity;
        }

        public IEnumerable<Courier> GetAllBusy()
        {
            var couriers = _dbContext
               .Couriers
               .Include(x => x.Transport)
               .Include(x => x.Status)
               .Where(x => x.Status == CourierStatus.Busy);

            return couriers;
        }

        public IEnumerable<Courier> GetAllReady()
        {
            var couriers = _dbContext
                .Couriers
                .Include(x => x.Transport)
                .Include(x => x.Status)
                .Where(x => x.Status == CourierStatus.Ready);

            return couriers;
        }

        public async Task<Courier> GetAsync(Guid courierId)
        {
            var courier = await _dbContext
               .Couriers
               .Include(x => x.Transport)
               .Include(x => x.Status)
               .FirstOrDefaultAsync(o => o.Id == courierId);
            
            return courier;
        }

        public void Update(Courier courier)
        {
            if (courier.Transport != null)
            {
                _dbContext.Attach(courier.Transport);
            }

            _dbContext.Attach(courier.Transport);

            if (courier.Status != null)
            {
                _dbContext.Attach(courier.Status);
            }

            _dbContext.Entry(courier).State = EntityState.Modified;

        }
    }
}
