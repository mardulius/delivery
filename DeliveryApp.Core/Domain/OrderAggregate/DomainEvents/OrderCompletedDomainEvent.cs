using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;

public sealed record OrderCompletedDomainEvent(Guid OrderId, string Status) : DomainEvent;
