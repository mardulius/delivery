using DeliveryApp.Core.Domain.CourierAggregate;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.CourierAggregate
{
    public class CourierEntityConfiguration : IEntityTypeConfiguration<Courier>
    {
        public void Configure(EntityTypeBuilder<Courier> builder)
        {
            builder.ToTable("couriers");

            builder.HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("id")
                .IsRequired();

            builder
                .Property(x => x.Name)
                .HasColumnName("name")
                .IsRequired();

            builder
                .HasOne(x => x.Transport)
                .WithMany()
                .IsRequired()
                .HasForeignKey("transport_id");

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



        }
    }
}
