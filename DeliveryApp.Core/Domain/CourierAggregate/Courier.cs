using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Courier : Aggregate
    {
        public static class Errors
        {
            public static Error TryStopWorkingWithIncompleteDelivery()
            {
                return new($"{nameof(Courier).ToLowerInvariant()}.try.stop.working.with.incomplete.delivery",
                    "Нельзя прекратить работу, если есть незавершенная доставка");
            }

            public static Error TryStartWorkingWhenAlreadyStarted()
            {
                return new($"{nameof(Courier).ToLowerInvariant()}.try.start.working.when.already.started",
                    "Нельзя начать работу, если ее уже начали ранее");
            }

            public static Error TryAssignOrderWhenNotAvailable()
            {
                return new($"{nameof(Courier).ToLowerInvariant()}.try.assign.order.when.not.available",
                    "Нельзя взять заказ в работу, если курьер не начал рабочий день");
            }

            public static Error TryAssignOrderWhenCourierHasAlreadyBusy()
            {
                return new($"{nameof(Courier).ToLowerInvariant()}.try.assign.order.when.courier.has.already.busy",
                    "Нельзя взять заказ в работу, если курьер уже занят");
            }
        }

        public string Name { get; private set; }
        public Transport Transport { get; private set; }     
        public Location Location { get; private set; }
        public CourierStatus Status { get; private set; }

        private Courier()
        {

        }
        private Courier(string name, Transport transport) : this()
        {
            Id = Guid.NewGuid();
            Name = name;
            Transport = transport;
            Location = Location.Create(1, 1).Value;
            Status = CourierStatus.NotAvailable;
        }

        public static Result<Courier, Error> Create(string name, Transport transport)
        {
            if (string.IsNullOrEmpty(name)) return GeneralErrors.ValueIsRequired(nameof(name));
            if (transport == null) return GeneralErrors.ValueIsRequired(nameof(transport));

            return new Courier(name, transport);
        }
        public Result<object, Error> Move(Location targetLocation)
        {
            if (targetLocation == null) return GeneralErrors.ValueIsRequired(nameof(targetLocation));
            if (Location == targetLocation) return new object();

            var cruisingRange = Transport.Speed; //запас хода

            var newX = Location.X;
            var newY = Location.Y;

            if (newX != targetLocation.X)
            {
                newX = Math.Min(Location.X + cruisingRange, targetLocation.X);
                var traveledX = targetLocation.X - Location.X; // сколько прошли по X
                cruisingRange -= traveledX;
            }

            // если ещё остался запас хода и курьер не в точке Y
            if (newY != targetLocation.Y && cruisingRange > 0)
            {
                newY = Math.Min(Location.Y + cruisingRange, targetLocation.Y);
            }

            var reachedLocation = Location.Create(newX, newY).Value;

            // Если курьер выполнял заказ, то он становится свободным 
            if (Status == CourierStatus.Busy && reachedLocation == targetLocation)
            {
                Status = CourierStatus.Ready;
            }

            Location = reachedLocation;
            return new object();
        }
        public Result<object, Error> StartWork()
        {
            if (Status == CourierStatus.Busy) return Errors.TryStartWorkingWhenAlreadyStarted();
            Status = CourierStatus.Ready;
            return new object();
        }
        public Result<object, Error> StopWork()
        {
            if (Status == CourierStatus.Busy) return Errors.TryStopWorkingWithIncompleteDelivery();
            Status = CourierStatus.NotAvailable;
            return new object();
        }

        public Result<object, Error> InWork()
        {
            if (Status == CourierStatus.NotAvailable) return Errors.TryAssignOrderWhenNotAvailable();
            if (Status == CourierStatus.Busy) return Errors.TryAssignOrderWhenCourierHasAlreadyBusy();
            Status = CourierStatus.Busy;
            return new object();
        }
        public Result<object, Error> CompleteOrder()
        {
            Status = CourierStatus.Ready;
            return new object();
        }
        public Result<double, Error> CalculateTimeToLocation(Location location)
        {
            if (location == null) return GeneralErrors.ValueIsRequired(nameof(location));

            var distance = Location.DistanceTo(location).Value;
            var time = (double)distance / Transport.Speed;
            return time;
        }

        public bool CanTakeOrder(Order order)
        {
            return Status == CourierStatus.Ready && order.Weight <= Transport.Capacity;
        }


    }
}
