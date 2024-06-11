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
        private readonly IGeoClient _geoClient;

        public CreateOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, IGeoClient geoClient)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _geoClient = geoClient; 
        }

        public async Task<bool> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        { 
            
           
            var order = await _orderRepository.GetAsync(request.BasketId);
            if (order != null) return true;

            // получить локацию из gRPC сервиса Geo
            var location = await _geoClient.GetGeolocationAsync(request.Address, cancellationToken);

            var weightCreateResult = Weight.Create(request.Weight);
            if (weightCreateResult.IsFailure) return false;
            var weight = weightCreateResult.Value;

            var orderCreateResult = Order.Create(request.BasketId, location, weight);
            if (orderCreateResult.IsFailure) return false;
            order = orderCreateResult.Value;
            _orderRepository.Add(order);

            return await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
