using CSharpFunctionalExtensions;
using Dapper;
using DeliveryApp.Core.Domain.OrderAggregate;
using MediatR;
using Npgsql;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders
{
    public class GetActiveOrdersHandler : IRequestHandler<GetActiveOrdersQuery, GetActiveOrdersResponse>
    {
        private readonly string _connectionString;

        public GetActiveOrdersHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<GetActiveOrdersResponse> Handle(GetActiveOrdersQuery request, CancellationToken cancellationToken)
        {
            using var postgres = new NpgsqlConnection(_connectionString);
            postgres.Open();

            var ordersFormDb = await postgres.QueryAsync<dynamic>("SELECT id, courier_id, location_x, location_y, weight, status FROM public.orders where status!=@status;",
                new { status = OrderStatus.Completed.Id });
            if (ordersFormDb.AsList().Count == 0)
                return null;

            var orders = new List<Order>();
            foreach (var item in ordersFormDb)
            {
                orders.Add(MapToOrder(item));
            }

            return new GetActiveOrdersResponse(orders);

        }


        private Order MapToOrder(dynamic result)
        {
            var location = new Location(result.location_x, result.location_y);
            var order = new Order (result.id, location);
            return order;
        }

    }
}
