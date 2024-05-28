using DeliveryApp.Core.Domain.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

            builder
              .OwnsOne(entity => entity.Capacity, c =>
              {
                  c.Property(v => v.Value).HasColumnName("capacity").IsRequired();
                  c.WithOwner();
              });

            builder.HasData(Transport.List().Select(x => new { x.Id, x.Name, x.Speed, x.Capacity.Value }));

        }
    }
}
