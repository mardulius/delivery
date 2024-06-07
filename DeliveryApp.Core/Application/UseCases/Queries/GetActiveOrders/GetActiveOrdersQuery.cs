using MediatR;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders
{
    public class GetActiveOrdersQuery : IRequest<GetActiveOrdersResponse>
    {
        public GetActiveOrdersQuery()
        {
        }
    }
}
