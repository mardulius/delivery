using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {

            var locationCreateResult = Location.CreateRandom();
            if (locationCreateResult.IsFailure) return false;
            var location = locationCreateResult.Value;

            var weightCreateResult = Weight.Create(request.Weight);
            if (weightCreateResult.IsFailure) return false;
            var weight = weightCreateResult.Value;

            var orderCreateResult = Order.Create(request.BasketId, location, weight);
            if (orderCreateResult.IsFailure) return false;
            var order = orderCreateResult.Value;

            _orderRepository.Add(order);

            return await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
