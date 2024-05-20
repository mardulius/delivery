using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel
{
    public class Location : ValueObject
    {
        public int X { get; }
        public int Y { get; }

        private Location()
        {
        }

        private Location(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static Result<Location, Error>  Create(int x, int y)
        {
            if (x < 1) return GeneralErrors.ValueIsInvalid(nameof(x));
            if (x > 10) return GeneralErrors.ValueIsInvalid(nameof(x));
            if (y < 1) return GeneralErrors.ValueIsInvalid(nameof(x));
            if (y > 10) return GeneralErrors.ValueIsInvalid(nameof(x));

            return new Location(x, y);           
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
