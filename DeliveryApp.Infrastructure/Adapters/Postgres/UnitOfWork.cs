using DeliveryApp.Infrastructure.Adapters.Postgres.Entities;
using MediatR;
using Newtonsoft.Json;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres
{
    public class UnitOfWork(AppDbContext dbContext) : IUnitOfWork
    {
        private readonly AppDbContext _dbContext = dbContext;
        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default)
        {
            await DispatchDomainEventsAsync();
            await _dbContext.SaveChangesAsync(cancellationToken);
            return true;
        }

        private async Task DispatchDomainEventsAsync()
        {
            // получить сущности с событиями
            var domainEntities = _dbContext
                .ChangeTracker
                .Entries<Aggregate>()
                .Where(x => x.Entity.GetDomainEvents().Count != 0)
                .ToList();

            // выбрать события
            var outboxMesaages = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .Select(x => new OutboxMessage
                {
                    Id = x.EventId,
                    OccuredOnUtc = DateTime.UtcNow,
                    Type = x.GetType().Name,
                    Content = JsonConvert.SerializeObject(x, new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
                }).ToList();

            // очистить события
            domainEntities
                .ForEach( x => x.Entity.ClearDomainEvents());

            await _dbContext.OutboxMessages.AddRangeAsync(outboxMesaages);

        }
    }
}
