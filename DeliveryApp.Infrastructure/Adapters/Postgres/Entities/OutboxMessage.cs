﻿namespace DeliveryApp.Infrastructure.Adapters.Postgres.Entities
{
    public class OutboxMessage
    {
        public Guid Id { get; set; }
        public string Type { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime OccuredOnUtc { get; set; }
        public DateTime? ProcessedOnUtc { get; set; }

    }
}
