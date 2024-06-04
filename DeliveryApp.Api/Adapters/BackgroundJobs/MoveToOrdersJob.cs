using DeliveryApp.Core.Application.UseCases.Commands.MoveToOrder;
using MediatR;
using Quartz;

namespace DeliveryApp.Api.Adapters.BackgroundJobs
{
    [DisallowConcurrentExecution]
    public class MoveToOrdersJob : IJob
    {
        private readonly IMediator _mediator;

        public MoveToOrdersJob(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        public async Task Execute(IJobExecutionContext context)
        {
            var moveToOrdersCommand = new MoveToOrderCommand();
            await _mediator.Send(moveToOrdersCommand);
        }
    }
}
