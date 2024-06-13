using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;
using DeliveryApp.Core.Ports;
using MediatR;


namespace DeliveryApp.Core.Application.DomainEventHendlers
{
    public class OrderCreatedDomainEventHandler : INotificationHandler<OrderCreatedDomainEvent>
    {
        private readonly IBusProducer _busProducer;

        public OrderCreatedDomainEventHandler(IBusProducer busProducer)
        {
            _busProducer = busProducer;
        }

        public async Task Handle(OrderCreatedDomainEvent notification, CancellationToken cancellationToken)
        {
            await _busProducer.PublishOrderCreatedDomainEvent(notification, cancellationToken);
        }
    }
}
