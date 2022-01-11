using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Infotecs.MobileMonitoring.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class StatisticsController : ControllerBase
    {
        private readonly IStatisticsService _statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            _statisticsService = statisticsService;
        }

        [HttpGet("list")]
        public Task<IEnumerable<StatisticsModel>> GetList(CancellationToken cancellationToken)
        {
            return _statisticsService.GetListAsync(cancellationToken);
        }
        [HttpPut("create")]
        public async Task<IActionResult> Create(StatisticsModel statisticsModel, CancellationToken cancellationToken)
        {
            statisticsModel.CreatedAt = DateTime.Now;
            await _statisticsService.CreateAsync(statisticsModel, cancellationToken);
            return NoContent();
        }
    }
}