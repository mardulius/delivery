
using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using Primitives;


namespace DeliveryApp.Core.DomainServices
{
    public class DispatchService : IDispatchService
    {
        public Result<Courier, Error> Dispatch(Order order, List<Courier> couriers)
        {
            if (!order.Status.Equals(OrderStatus.Created)) return GeneralErrors.ValueIsInvalid("не правильный статус заказа");

            var couriersReady = couriers.Where(c => c.Transport.CanAllocate(order.Weight).Value);

            var scores = new List<Score>();

            foreach(var courier in couriersReady)
            {
                var time = courier.CalculateTimeToLocation(order.Location).Value;
                scores.Add(new Score { Courier = courier, TimeToLocation = time });
            }

            var minTimeCourier = scores.MinBy(x => x.TimeToLocation).Courier;

            order.Assign(minTimeCourier);

            return minTimeCourier;
        }

        public class Score
        {
            public Courier Courier { get; set; }
            public double TimeToLocation { get; set; }
        }


    }
}
