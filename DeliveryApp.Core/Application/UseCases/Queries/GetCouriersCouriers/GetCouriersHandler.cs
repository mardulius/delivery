using Dapper;
using MediatR;
using Npgsql;

namespace DeliveryApp.Core.Application.UseCases.Queries.GetCouriers
{
    public class GetCouriersHandler : IRequestHandler<GetCouriersQuery, GetCouriersResponse>
    {
        private readonly string _connectionString;

        public GetCouriersHandler(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<GetCouriersResponse> Handle(GetCouriersQuery request, CancellationToken cancellationToken)
        {

            using var postgres = new NpgsqlConnection(_connectionString);
            postgres.Open();

            var result = await postgres.QueryAsync<dynamic>(
                @"SELECT id, name, location_x, location_y FROM public.couriers;"
                , new { });

            if (result.AsList().Count == 0)
                return null;

            var couriers = new List<Courier>();
            foreach (var item in result)
            {
                couriers.Add(MapToCourier(item));
            }

            return new GetCouriersResponse(couriers);

        }

        private Courier MapToCourier(dynamic result)
        {
            var location = new Location(result.location_x, result.location_y );
            var courier = new Courier(result.id, result.name, location );
            return courier;
        }

    }
}
