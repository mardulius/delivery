using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel
{
    public class Location : ValueObject
    {
        public int X { get; }
        public int Y { get; }

        public static readonly Location MinLocation = new(1, 1);
        public static readonly Location MaxLocation = new(10, 10);

        private Location()
        {
        }

        private Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Result<Location, Error> Create(int x, int y)
        {
            if (x < MinLocation.X || x > MaxLocation.X) return GeneralErrors.ValueIsInvalid(nameof(x));
            if (y < MinLocation.Y || y > MaxLocation.Y) return GeneralErrors.ValueIsInvalid(nameof(y));


            return new Location(x, y);           
        }

        public static Result<Location, Error> CreateRandom()
        {
            var random = new Random();
            var x = random.Next(MinLocation.X, MaxLocation.X);
            var y = random.Next(MinLocation.X, MaxLocation.Y);
            var location = new Location(x, y);
            return location;
        }



        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return X;
            yield return Y;
        }

        public Result<int, Error> DistanceTo(Location targetLocation)
        {
            var diffX = Math.Abs(X - targetLocation.X);
            var diffY = Math.Abs(Y - targetLocation.Y);

            return diffX + diffY;
        } 

    }

}
