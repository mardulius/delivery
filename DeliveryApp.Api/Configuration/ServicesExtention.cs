using DeliveryApp.Core.Application.UseCases.Commands.AssingOrder;
using DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Commands.MoveToOrder;
using DeliveryApp.Core.Application.UseCases.Commands.StartWork;
using DeliveryApp.Core.Application.UseCases.Commands.StopWork;
using DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders;
using DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;
using MediatR;

namespace DeliveryApp.Api.Configuration
{
    public static class ServicesExtention
    {

        public static IServiceCollection AddConfiguredMediator(this IServiceCollection services, string connectionString)
        {
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Startup>());

            // Commands
            services.AddTransient<IRequestHandler<CreateOrderCommand, bool>, CreateOrderHandler>();
            services.AddTransient<IRequestHandler<MoveToOrderCommand, bool>, MoveToOrderHandler>();
            services.AddTransient<IRequestHandler<AssignOrderCommand, bool>, AssignOrderHandler>();
            services.AddTransient<IRequestHandler<StartWorkCommand, bool>, StartWorkHandler>();
            services.AddTransient<IRequestHandler<StopWorkCommand, bool>, StopWorkHandler>();

            // Queries
            services.AddTransient<IRequestHandler<GetActiveOrdersQuery, GetActiveOrdersResponse>>(x =>
                new GetActiveOrdersHandler(connectionString));
            services.AddTransient<IRequestHandler<GetCouriersQuery, GetCouriersResponse>>(x =>
                new GetCouriersHandler(connectionString));

            return services;
        }
    }
}
