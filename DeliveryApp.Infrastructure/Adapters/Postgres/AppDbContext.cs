using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.CourierAggregate;
using DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.OrderAggregate;
using Microsoft.EntityFrameworkCore;

namespace DeliveryApp.Infrastructure.Adapters.Postgres
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Courier> Couriers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderEntityConfiguration());
            modelBuilder.ApplyConfiguration(new OrderStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourierEntityConfiguration());
            modelBuilder.ApplyConfiguration(new CourierStatusEntityConfiguration());
            modelBuilder.ApplyConfiguration(new TransportEntityConfiguration());
            
            //modelBuilder.Entity<OrderStatus>(b =>
            //{
            //    var allStatuses = OrderStatus.List();
            //    b.HasData(allStatuses.Select(c => new { c.Id, c.Name }));
            //});
            //modelBuilder.Entity<CourierStatus>(b =>
            //{
            //    var allStatuses = CourierStatus.List();
            //    b.HasData(allStatuses.Select(c => new { c.Id, c.Name }));
            //});
            //modelBuilder.Entity<Transport>(b =>
            //{
            //    var allTransports = Transport.List();
            //    b.HasData(allTransports.Select(c => new { c.Id, c.Name, c.Speed, c.Capacity.Value }));
            //});


        }

    }
}
