
using DeliveryApp.Core.Domain.OrderAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.OrderAggregate
{
    public class OrderEntityConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .HasColumnName("Id")
                .ValueGeneratedNever()
                .IsRequired();

            builder
                .Property(x => x.CourierId)
                .HasColumnName("courier_id")
                .IsRequired(false);

            builder
                .HasOne(x => x.Status)
                .WithMany()
                .IsRequired()
                .HasForeignKey("status_id");

            builder
               .OwnsOne(entity => entity.Location, l =>
                {
                    l.Property(x => x.X).HasColumnName("location_x").IsRequired();
                    l.Property(y => y.Y).HasColumnName("location_y").IsRequired();
                    l.WithOwner();
                });

            builder
               .OwnsOne(entity => entity.Weight, w =>
                {
                    w.Property(c => c.Value).HasColumnName("weight").IsRequired();
                    w.WithOwner();
                });

        }
    }
}
