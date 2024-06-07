using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.MoveToOrder
{
    public class MoveToOrderHandler : IRequestHandler<MoveToOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public MoveToOrderHandler(IOrderRepository orderRepository, ICourierRepository courierRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _courierRepository = courierRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(MoveToOrderCommand request, CancellationToken cancellationToken)
        {
            var orders = _orderRepository.GetAllAssigned();
            if (!orders.Any())
                return false;

            foreach (var order in orders)
            {
                var courier = await _courierRepository.GetAsync((Guid)order.CourierId);
                courier.Move(order.Location);
                if (courier.Status == CourierStatus.Ready)
                {
                    order.Complete();
                }
                _courierRepository.Update(courier);
                _orderRepository.Update(order);
            }
            return await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
