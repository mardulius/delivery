
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.StartWork
{
    public class StartWorkCommand : IRequest<bool>
    {
        public Guid CourierId { get; private set; }
        public StartWorkCommand(Guid courierId)
        {
            if (courierId == Guid.Empty) throw new ArgumentException(nameof(courierId));
            CourierId = courierId;
        }

        
    }
}
