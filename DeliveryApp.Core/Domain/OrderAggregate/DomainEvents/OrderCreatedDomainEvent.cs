using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;

public sealed record OrderCreatedDomainEvent(Guid OrderId, string Status) : DomainEvent;
