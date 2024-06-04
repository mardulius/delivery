using Api.Controllers;
using DeliveryApp.Core.Application.UseCases.Commands.CreateOrder;
using DeliveryApp.Core.Application.UseCases.Commands.StartWork;
using DeliveryApp.Core.Application.UseCases.Commands.StopWork;
using DeliveryApp.Core.Application.UseCases.Queries.GetActiveOrders;
using DeliveryApp.Core.Application.UseCases.Queries.GetCouriers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace DeliveryApp.Api.Adapters.Http.Contract
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliveryController : DefaultApiController
    {
        private readonly IMediator _mediator;

        public DeliveryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public override async Task<IActionResult> CreateOrder()
        {
            var basketId = Guid.NewGuid();
            var address = "Профсоюзная 12";
            var weight = 12; 

            var createOrderCommand = new CreateOrderCommand(basketId, address, weight);
            var result = await _mediator.Send(createOrderCommand);
            if (result) return Ok();
            return Conflict();
        }

        public override async Task<IActionResult> GetCouriers()
        {
            var getCouriersQuery = new GetCouriersQuery();
            var result = await _mediator.Send(getCouriersQuery);
            return Ok(result);
        }

        public override async Task<IActionResult> GetOrders()
        {
            var getOrdersQuery = new GetActiveOrdersQuery();
            var result = await _mediator.Send(getOrdersQuery);
            return Ok(result);
        }

        public override async Task<IActionResult> StartWork([FromRoute(Name = "courierId"), Required] Guid courierId)
        {
            var startWorkCommand = new StartWorkCommand(courierId);
            var result = await _mediator.Send(startWorkCommand);
            if (result) return Ok();
            return Conflict();

        }

        public override async Task<IActionResult> StopWork([FromRoute(Name = "courierId"), Required] Guid courierId)
        {
            var stopWorkCommand = new StopWorkCommand(courierId);
            var result = await _mediator.Send(stopWorkCommand);
            if (result) return Ok();
            return Conflict();
        }
    }
}
