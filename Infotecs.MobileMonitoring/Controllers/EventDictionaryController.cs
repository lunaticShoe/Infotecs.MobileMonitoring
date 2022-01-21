using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Contracts;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.MobileMonitoring.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class EventDictionaryController : ControllerBase
    {
        private readonly IEventDictionaryService eventDictionaryService;

        public EventDictionaryController(IEventDictionaryService eventDictionaryService)
        {
            this.eventDictionaryService = eventDictionaryService;
        }
        [HttpGet("list")]
        public async Task<ICollection<EventDictionaryContract>> GetListAsync(CancellationToken cancellationToken = default)
        {
            return (await eventDictionaryService.GetListAsync(cancellationToken))
                .Adapt<ICollection<EventDictionaryContract>>();
        }

        [HttpPut("upsert")]
        public async Task<IActionResult> UpsertAsync(EventDictionaryContract contract, CancellationToken cancellationToken = default)
        {
            await eventDictionaryService.UpsertAsync(contract.Adapt<EventDictionaryModel>(), cancellationToken);
            return NoContent();
        }

    }
}