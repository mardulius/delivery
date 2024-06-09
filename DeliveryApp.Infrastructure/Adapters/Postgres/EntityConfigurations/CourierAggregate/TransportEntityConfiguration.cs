using DeliveryApp.Core.Domain.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection.Emit;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.CourierAggregate
{
    public class TransportEntityConfiguration : IEntityTypeConfiguration<Transport>
    {
        public void Configure(EntityTypeBuilder<Transport> builder)
        {
            builder.ToTable("transports");

            builder.HasKey(entity => entity.Id);

            builder
                .Property(entity => entity.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder
                .Property(entity => entity.Name)
                .HasColumnName("name")
                .IsRequired();

            builder
                .Property(entity => entity.Speed)
                .HasColumnName("speed")
                .IsRequired();
            
            var allTransports = Transport.List();

            builder
              .OwnsOne(entity => entity.Capacity, c =>
              {
                  c.HasData(allTransports.Select(c => new
                  {
                      TransportId = c.Id,
                      c.Capacity.Value
                  }));
                  c.Property(v => v.Value).HasColumnName("capacity").IsRequired();
                  c.WithOwner();
              }).HasData(allTransports.Select(c => new
              {
                  c.Id,
                  c.Name,
                  c.Speed
              }));


        }
    }
}
