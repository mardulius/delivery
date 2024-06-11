﻿using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;

namespace DeliveryApp.Core.Ports;

public interface IBusProducer
{
    Task PublishOrderCreatedDomainEvent(OrderCreatedDomainEvent orderCreatedDomainEvent, 
        CancellationToken cancellationToken);

    Task PublishOrderAssignedDomainEvent(OrderAssignedDomainEvent notification,
        CancellationToken cancellationToken);

    Task PublishOrderCompletedDomainEvent(OrderCompletedDomainEvent notification,
        CancellationToken cancellationToken);
}
