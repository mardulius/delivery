using CSharpFunctionalExtensions;
using Primitives;

namespace DeliveryApp.Core.Domain.SharedKernel
{
    public class Weight : ValueObject
    {
        public int Value { get; }
        private Weight()
        {
        }

        private Weight(int value)
        {
            Value = value;
        }

        public static Result<Weight, Error> Create(int value)
        {
            if (value <= 0) return GeneralErrors.ValueIsRequired(nameof(value));
            return new Weight(value);
        }

        public static bool operator <(Weight first, Weight second)
        {
            var result = first.Value < second.Value;
            return result;
        }

        public static bool operator >(Weight first, Weight second)
        {
            var result = first.Value > second.Value;
            return result;
        }

        public static bool operator <=(Weight first, Weight second)
        {
            var result = first.Value <= second.Value;
            return result;
        }

        public static bool operator >=(Weight first, Weight second)
        {
            var result = first.Value >= second.Value;
            return result;
        }

        protected override IEnumerable<IComparable> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
