using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using Primitives;

namespace DeliveryApp.Core.DomainServices
{
    public interface IDispatchService
    {
        Result<Courier, Error> Dispatch(Order order, List<Courier> couriers);
    }
}
