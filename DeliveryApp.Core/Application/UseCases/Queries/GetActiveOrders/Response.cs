
namespace DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders
{
    public class Response
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public Response(List<Order> orders)
        {
            Orders.AddRange(orders);
        }
    }
    public record Order(Guid Id, Location Location);
    public record Location(int X, int Y);
}
