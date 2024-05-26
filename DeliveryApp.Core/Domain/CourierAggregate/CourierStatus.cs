

using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.CourierAggregate
{
    public class CourierStatus : Entity<int>
    {
        
        public static readonly CourierStatus Ready = new(2, nameof(Ready).ToLowerInvariant());
        public static readonly CourierStatus Busy = new(3, nameof(Busy).ToLowerInvariant());
        public static readonly CourierStatus NotAvailable = new(1, nameof(NotAvailable).ToLowerInvariant());

        public string Name { get; }

        private CourierStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }
        public static IEnumerable<CourierStatus> List()
        {
            yield return Ready;
            yield return Busy;
            yield return NotAvailable;
        }


    }
}
