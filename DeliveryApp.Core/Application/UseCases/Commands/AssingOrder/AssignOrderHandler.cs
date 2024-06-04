
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.DomainServices;
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.AssingOrder
{
    public class AssignOrderHandler : IRequestHandler<AssignOrderCommand, bool>

    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICourierRepository _courierRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IDispatchService _dispatchService;

        public AssignOrderHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork, ICourierRepository courierRepository,
            IDispatchService dispatchService)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _courierRepository = courierRepository;
            _dispatchService = dispatchService;
        }

        public async Task<bool> Handle(AssignOrderCommand request, CancellationToken cancellationToken)
        {
            var order = _orderRepository.GetAllNotAssigned().FirstOrDefault();
            if (order == null) return false;
            var couriers = _courierRepository.GetAllReady().ToList();
            if (couriers.Count == 0) return false;

            var dispatchResult = _dispatchService.Dispatch(order, couriers);
            if(dispatchResult.IsFailure) return false;
            var dispatchedCourier = dispatchResult.Value;

            _courierRepository.Update(dispatchedCourier);
            _orderRepository.Update(order);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);

            return true;
        }

    }
}
