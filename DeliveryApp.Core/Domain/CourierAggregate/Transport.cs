using CSharpFunctionalExtensions;
using DeliveryApp.Core.Domain.SharedKernel;
using Primitives;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class Transport : Entity<int>
    {
        public static readonly Transport Pedestrian = new(1, nameof(Pedestrian).ToLowerInvariant(), 1,
           Weight.Create(1).Value);

        public static readonly Transport
            Bicycle = new(2, nameof(Bicycle).ToLowerInvariant(), 2, Weight.Create(4).Value);

        public static readonly Transport
            Scooter = new(3, nameof(Scooter).ToLowerInvariant(), 3, Weight.Create(6).Value);

        public static readonly Transport
            Car = new(4, nameof(Car).ToLowerInvariant(), 4, Weight.Create(8).Value);


        public string Name { get; }       
        public int Speed { get; }
        public Weight Capacity { get; }

        private Transport(int id, string name, int speed, Weight capacity)
        {
            Id = id;
            Name = name;
            Speed = speed;
            Capacity = capacity;
        }

        public static IEnumerable<Transport> List()
        {
            yield return Pedestrian;
            yield return Bicycle;
            yield return Scooter;
            yield return Car;
        }

        public Result<bool, Error> CanAllocate(Weight weight)
        {
            return Capacity >= weight;
        }


    }
}
