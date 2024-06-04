
namespace DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders
{
    public class GetActiveOrdersResponse
    {
        public List<Order> Orders { get; set; } = new List<Order>();

        public GetActiveOrdersResponse(List<Order> orders)
        {
            Orders.AddRange(orders);
        }
    }
    public record Order(Guid Id, Location Location);
    public record Location(int X, int Y);
}
