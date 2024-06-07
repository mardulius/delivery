
using DeliveryApp.Core.Ports;
using MediatR;
using Primitives;

namespace DeliveryApp.Core.Application.UseCases.Commands.StartWork
{
    public class StartWorkHandler : IRequestHandler<StartWorkCommand, bool>
    {
        private readonly ICourierRepository _courierRepository;
        private readonly IUnitOfWork _unitOfWork;

        public StartWorkHandler(ICourierRepository courierRepository, IUnitOfWork unitOfWork)
        {
            _courierRepository = courierRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<bool> Handle(StartWorkCommand request, CancellationToken cancellationToken)
        {
            //\воостановить модель
            var courier = await _courierRepository.GetAsync(request.CourierId);
            if (courier == null) return false;

            //изменить статус

            var result = courier.StartWork();

            if(result.IsFailure) return false;

            //сохранить изменения
            _courierRepository.Update(courier);
            await _unitOfWork.SaveEntitiesAsync(cancellationToken);
            return true;
        }
    }
}
