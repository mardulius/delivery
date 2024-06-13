using DeliveryApp.Infrastructure.Adapters.Postgres.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DeliveryApp.Infrastructure.Adapters.Postgres.EntityConfigurations.OrderAggregate
{
    public class OutboxEntityTypeConfiguration : IEntityTypeConfiguration<OutboxMessage>
    {
        public void Configure(EntityTypeBuilder<OutboxMessage> builder)
        {
            builder
             .ToTable("outbox");
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");

            builder
                .Property(x => x.Type)
                .HasColumnName("type")
                .IsRequired();

            builder
                .Property(x => x.Content)
                .HasColumnName("content")
                .IsRequired();

            builder
                .Property(x => x.OccuredOnUtc)
                .HasColumnName("occured_on_utc")
                .IsRequired();

            builder
                .Property(x => x.ProcessedOnUtc)
                .HasColumnName("processed_on_utc")
                .IsRequired(false);

        }
    }
}
