using Confluent.Kafka;
using DeliveryApp.Core.Domain.OrderAggregate.DomainEvents;
using DeliveryApp.Core.Ports;
using Newtonsoft.Json;
using OrderStatusChanged;
using Primitives.Extensions;

namespace DeliveryApp.Infrastructure.Adapters.Kafka.OrderStatusChanged
{
    public class Producer : IBusProducer
    {
        private readonly ProducerConfig _config;
        private readonly string _topicName = "order.status.changed";

        public Producer(string hostAddress)
        {
            _config = new ProducerConfig
            {
                BootstrapServers = hostAddress
            };

        }

        public async Task PublishOrderCreatedDomainEvent(OrderCreatedDomainEvent orderCreatedDomainEvent, CancellationToken cancellationToken)
        {
            var orderStatusChangedIntegrationEvent = new OrderStatusChangedIntegrationEvent
            {
                OrderId = orderCreatedDomainEvent.OrderId.ToString(),
                OrderStatus = orderCreatedDomainEvent.Status.ToEnum<OrderStatus>(),
            };

            var message = new Message<string, string>
            {
                Key = orderCreatedDomainEvent.EventId.ToString(),
                Value = JsonConvert.SerializeObject(orderStatusChangedIntegrationEvent)
            };

            using var producer = new ProducerBuilder<string, string>(_config).Build();
            await producer.ProduceAsync(_topicName, message, cancellationToken);

        }

        public async Task PublishOrderAssignedDomainEvent(OrderAssignedDomainEvent orderAssignedDomainEvent, CancellationToken cancellationToken)
        {
            var orderStatusChangedIntegrationEvent = new OrderStatusChangedIntegrationEvent
            {
                OrderId = orderAssignedDomainEvent.OrderId.ToString(),
                OrderStatus = orderAssignedDomainEvent.Status.ToEnum<OrderStatus>(),
            };

            var message = new Message<string, string>
            {
                Key = orderAssignedDomainEvent.EventId.ToString(),
                Value = JsonConvert.SerializeObject(orderStatusChangedIntegrationEvent)
            };

            using var producer = new ProducerBuilder<string, string>(_config).Build();
            await producer.ProduceAsync(_topicName, message, cancellationToken);
        }

        public async Task PublishOrderCompletedDomainEvent(OrderCompletedDomainEvent orderComlitedDomainEvent, CancellationToken cancellationToken)
        {
            var orderStatusChangedIntegrationEvent = new OrderStatusChangedIntegrationEvent
            {
                OrderId = orderComlitedDomainEvent.OrderId.ToString(),
                OrderStatus = orderComlitedDomainEvent.Status.ToEnum<OrderStatus>(),
            };

            var message = new Message<string, string>
            {
                Key = orderComlitedDomainEvent.EventId.ToString(),
                Value = JsonConvert.SerializeObject(orderStatusChangedIntegrationEvent)
            };

            using var producer = new ProducerBuilder<string, string>(_config).Build();
            await producer.ProduceAsync(_topicName, message, cancellationToken);
        }
    }
}


