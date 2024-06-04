
namespace DeliveryApp.Core.Application.UseCases.Queries.GetCouriers
{
    public class GetCouriersResponse
    {
        public List<Courier> Couriers { get; set; } = new List<Courier>(); 
        public GetCouriersResponse(List<Courier> couriers)
        {
            Couriers.AddRange(couriers);
        }
    }

    public record Courier(Guid Id, string Name, Location Location);
    public record Location(int X, int Y);
}
