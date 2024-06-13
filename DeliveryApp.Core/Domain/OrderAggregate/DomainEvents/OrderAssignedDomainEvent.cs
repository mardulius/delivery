using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;

public sealed record OrderAssignedDomainEvent(Guid OrderId, string Status) : DomainEvent;
