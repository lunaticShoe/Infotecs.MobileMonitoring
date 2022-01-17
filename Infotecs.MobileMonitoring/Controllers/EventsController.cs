using Infotecs.MobileMonitoring.Contracts;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.MobileMonitoring.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventsController(IEventService eventService)
        {
            this.eventService = eventService;
        }
        [HttpGet("list/{statisticsId:guid}")]
        public async Task<ICollection<EventContract>> GetListAsync(
            Guid statisticsId, CancellationToken cancellationToken = default)
        {
            return (await eventService.GetListAsync(statisticsId, cancellationToken))
                .Adapt<ICollection<EventContract>>();
        }
        //
        // [HttpPut("create")]
        // public async Task<IActionResult> CreateAsync(EventCreateContract contract, CancellationToken cancellationToken = default)
        // {
        //     await eventService.CreateAsync(contract.Adapt<EventModel>(), cancellationToken);
        //     return NoContent();
        // }
    }
}