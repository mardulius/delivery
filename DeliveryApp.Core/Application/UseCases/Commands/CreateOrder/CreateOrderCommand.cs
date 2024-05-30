
using MediatR;
using System.Net;

namespace DeliveryApp.Core.Application.UseCases.Commands.CreateOrder
{
    public class CreateOrderCommand : IRequest<bool>
    {
        public Guid BasketId { get; }
        public string Address { get; }
        public int Weight { get; }

        public CreateOrderCommand(Guid basketId, string address, int weight)
        {           
            BasketId = basketId;
            Address = address;
            Weight = weight;

        }
    }
}
