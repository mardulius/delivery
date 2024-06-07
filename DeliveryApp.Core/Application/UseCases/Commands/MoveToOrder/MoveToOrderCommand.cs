
using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Commands.MoveToOrder
{
    public class MoveToOrderCommand : IRequest<bool>
    {
        public MoveToOrderCommand()
        {
        }
    }
}
