using DeliveryApp.Core.Domain.CourierAggregate;
using DeliveryApp.Core.Domain.OrderAggregate;
using DeliveryApp.Core.Domain.SharedKernel;
using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Repositories;
using FluentAssertions;
using MediatR;
using Microsoft.EntityFrameworkCore;
using NSubstitute;
using Testcontainers.PostgreSql;
using Xunit;

namespace DeliveryApp.IntegrationTests.Repositories
{
    public class OrderRepositoryTests : IAsyncLifetime
    {
        private AppDbContext _dbContext;
        private UnitOfWork _unitOfWork;
        private OrderRepository _orderRepository;
        private readonly Location _location;
        private readonly Weight _weight;
        private readonly IMediator _mediator;



        private readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
          .WithImage("postgres:14.7")
          .WithDatabase("order")
          .WithUsername("username")
          .WithPassword("secret")
          .WithCleanUp(true)
          .Build();

        public OrderRepositoryTests()
        {
            _mediator = Substitute.For<IMediator>();

            var weightCreateResult = Weight.Create(4);
            weightCreateResult.IsSuccess.Should().BeTrue();
            _weight = weightCreateResult.Value;

            var locationCreateResult = Location.Create(1, 1);
            locationCreateResult.IsSuccess.Should().BeTrue();
            _location = locationCreateResult.Value;
        }

        public async Task InitializeAsync()
        {
            await _postgresContainer.StartAsync();

            var contextOptions = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(_postgresContainer.GetConnectionString(),
                npgsqlOptionsAction: sqlOptions => { sqlOptions.MigrationsAssembly("DeliveryApp.Infrastructure"); }).Options;

            _dbContext = new AppDbContext(contextOptions);

            _dbContext.Database.EnsureCreated();

            _orderRepository = new(_dbContext);
            _unitOfWork = new(_dbContext, _mediator);



        }
        public async Task DisposeAsync()
        {
            await _postgresContainer.DisposeAsync().AsTask();
        }

        [Fact]
        public async Task CanAddOrder()
        {
            //arrange
            var order = Order.Create(Guid.NewGuid(), _location, _weight).Value;

            //act
            _orderRepository.Add(order);
            await _unitOfWork.SaveEntitiesAsync();

            //assert
            var orderFromDb = await _orderRepository.GetAsync(order.Id);
            order.Should().BeEquivalentTo(orderFromDb);
        }


        [Fact]
        public async Task CanUpdateOrder()
        {
            //arrange
            var order = Order.Create(Guid.NewGuid(), _location, _weight).Value;
            var courier = Courier.Create("Sergey", Transport.Scooter).Value;
            
            _orderRepository.Add(order);
            await _unitOfWork.SaveEntitiesAsync();

            //act
            order.Assign(courier);
            _orderRepository.Update(order);
            await _unitOfWork.SaveEntitiesAsync();

            //assert
            var orderFromDb = await _orderRepository.GetAsync(order.Id);
            orderFromDb.Should().BeEquivalentTo(order);

        }
    }
}
