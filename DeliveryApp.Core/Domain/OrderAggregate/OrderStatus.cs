using CSharpFunctionalExtensions;

namespace DeliveryApp.Core.Domain.OrderAggregate
{
    public class OrderStatus : Entity<int>
    {
        public static readonly OrderStatus Created = new(1, nameof(Created).ToLowerInvariant());
        public static readonly OrderStatus Assigned = new(1, nameof(Assigned).ToLowerInvariant());
        public static readonly OrderStatus Completed = new(1, nameof(Completed).ToLowerInvariant());
        
        public string Name { get;}

        private OrderStatus(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public static IEnumerable<OrderStatus> List()
        {
            yield return Created;
            yield return Assigned;
            yield return Completed;
        }


    }

}
