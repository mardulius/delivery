using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.OrderAggregate
{
    public class Order : Aggregate
    {

        public static class Errors
        {
            public static Error CantCompletedNotAssignedOrder()
            {
                return new($"{nameof(Order).ToLowerInvariant()}.cant.completed.not.sssigned.order",
                    "Нельза завершить заказ, который не был назначен");
            }

            public static Error CantAssignOrderToBusyCourier(Guid courierId)
            {
                return new($"{nameof(Order).ToLowerInvariant()}.cant.assign.order.to.busy.courier",
                    $"Нельза назначить заказ на курьера, который занят. Id курьера = {courierId}");
            }
        }



        public Guid? CourierId { get; private set; }
        public Location Location { get; private set; }
        public Weight Weight { get; private set; }  
        public OrderStatus Status { get; private set; }

        public Order()
        {
            
        }
        private Order(Guid backetId, Location location, Weight weight) : this()
        { 
            Id = backetId;
            Location = location;
            Weight = weight;
            Status = OrderStatus.Created;            
        }

        public static Result<Order, Error> Create(Guid backetId, Location location, Weight weight)
        {
            if (backetId == Guid.Empty) return GeneralErrors.ValueIsRequired(nameof(backetId));
            if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));
            if (weight == null) return GeneralErrors.ValueIsRequired(nameof(weight));


            return new Order(backetId, location, weight);
        }


        public Result<object, Error> Assign(Courier courier)
        {
            if (courier == null) return GeneralErrors.ValueIsRequired(nameof(courier));
            if (courier.Status == CourierAggregate.CourierStatus.Busy)
                return Errors.CantAssignOrderToBusyCourier(courier.Id);

            CourierId = courier.Id;
            Status = OrderStatus.Assigned;
            courier.InWork();
            return new object();
        }

        public Result<object, Error> Complete()
        {
            if (Status != OrderStatus.Assigned) return GeneralErrors.ValueIsInvalid(nameof(OrderStatus.Assigned));
            Status = OrderStatus.Completed;
            return new object();
        }

    }

}
