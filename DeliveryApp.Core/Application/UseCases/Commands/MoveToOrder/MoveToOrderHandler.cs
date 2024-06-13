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
            var assignedOrders = _orderRepository.GetAllAssigned().ToList();
            if (assignedOrders.Count == 0)
                return false;

            foreach (var order in assignedOrders)
            {

                if (order.CourierId == null)
                    return false;

                var courier = await _courierRepository.GetAsync((Guid) order.CourierId);
                if (courier == null) return false;

                var courierMoveResult = courier.Move(order.Location);
                if (courierMoveResult.IsFailure) return false;


                if (order.Location == courier.Location)
                {
                    order.Complete();
                    courier.CompleteOrder();
                }
                _courierRepository.Update(courier);
                _orderRepository.Update(order);
            }
            return await _unitOfWork.SaveEntitiesAsync(cancellationToken);
        }
    }
}
