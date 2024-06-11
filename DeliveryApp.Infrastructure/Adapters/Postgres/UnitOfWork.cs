using MediatR;
using Primitives;

namespace DeliveryApp.Infrastructure.Adapters.Postgres
{
    public class UnitOfWork(AppDbContext dbContext, IMediator mediator) : IUnitOfWork
    {
        private readonly AppDbContext _dbContext = dbContext;
        private readonly IMediator _mediator = mediator;

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
                .Where(x => x.Entity.GetDomainEvents().Any())
                .ToList();

            // выбрать события
            var domainEvents = domainEntities
                .SelectMany(x => x.Entity.GetDomainEvents())
                .ToList();

            // очистить события
            domainEntities
                .ForEach( x => x.Entity.ClearDomainEvents());

            foreach(var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);
            }

        }
    }
}
