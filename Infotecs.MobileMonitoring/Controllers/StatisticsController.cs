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
        private readonly IStatisticsService statisticsService;

        public StatisticsController(IStatisticsService statisticsService)
        {
            this.statisticsService = statisticsService;
        }

        [HttpGet("list")]
        public Task<ICollection<StatisticsModel>> GetList(CancellationToken cancellationToken)
        {
            return statisticsService.GetListAsync(cancellationToken);
        }
        [HttpPut("create")]
        public async Task<IActionResult> Create(StatisticsModel statisticsModel, CancellationToken cancellationToken)
        {
            statisticsModel.CreatedAt = DateTime.Now;
            await statisticsService.CreateAsync(statisticsModel, cancellationToken);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(StatisticsModel statisticsModel, CancellationToken cancellationToken)
        {
            await statisticsService.UpdateAsync(statisticsModel,cancellationToken);
            return NoContent();
        }
    }
}