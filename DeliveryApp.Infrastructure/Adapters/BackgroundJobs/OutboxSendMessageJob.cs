using DeliveryApp.Infrastructure.Adapters.Postgres;
using DeliveryApp.Infrastructure.Adapters.Postgres.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Primitives;
using Quartz;

namespace DeliveryApp.Infrastructure.Adapters.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class OutboxSendMessageJob(AppDbContext appDbContext, IPublisher publisher) : IJob
    {
        private readonly AppDbContext _dbContext = appDbContext;
        private readonly IPublisher _publisher = publisher;

        public async Task Execute(IJobExecutionContext context)
        {
            var outboxMessages = await _dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .OrderByDescending(o => o.OccuredOnUtc)
            .Take(20)
            .ToListAsync(context.CancellationToken);

            if (outboxMessages.Count != 0)
            {
                foreach (var outboxMessage in outboxMessages)
                {
                    var domainEvent = JsonConvert.DeserializeObject<DomainEvent>(outboxMessage.Content,
                        new JsonSerializerSettings
                        {
                            TypeNameHandling = TypeNameHandling.All
                        });


                    await _publisher.Publish(domainEvent, context.CancellationToken);
                    outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
                }
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}
