using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.StopWork
{
    public class StopWorkCommand : IRequest<bool>
    {
        public Guid CourierId { get; private set; }
        public StopWorkCommand(Guid courierId)
        {
            if (courierId == Guid.Empty) throw new ArgumentException(nameof(courierId));
            CourierId = courierId;
        }

    }
}
