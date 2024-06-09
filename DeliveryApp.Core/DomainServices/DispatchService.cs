
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
            if (order == null) return GeneralErrors.ValueIsRequired(nameof(order));
            if (couriers == null || couriers.Count == 0) return GeneralErrors.InvalidLength(nameof(couriers));

            var scores = new List<Score>();
            
            
            var minTimeToPoint = double.MaxValue;
            Courier fastestCourier = null;
            
            foreach (var courier in couriers.Where(x => x.CanTakeOrder(order)))
            {
                var courierCalculateTimeToLocationResult = courier.CalculateTimeToLocation(order.Location);
                if (courierCalculateTimeToLocationResult.IsFailure) return courierCalculateTimeToLocationResult.Error;
                var timeToLocation = courierCalculateTimeToLocationResult.Value;

                if (timeToLocation < minTimeToPoint)
                {
                    minTimeToPoint = timeToLocation;
                    fastestCourier = courier;
                }
            }

            order.Assign(fastestCourier);

            return fastestCourier;
        }

        public class Score
        {
            public Courier Courier { get; set; }
            public double TimeToLocation { get; set; }
        }


    }
}
