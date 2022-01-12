using System;
using System.Collections.Generic;
using System.Linq;
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
        public async Task<IActionResult> Create(StatisticsContract statisticsContract, CancellationToken cancellationToken)
        {
            //statisticsModel.CreatedAt = DateTime.Now;
            var statisticsModel = statisticsContract.Adapt<StatisticsModel>();
            await statisticsService.CreateAsync(statisticsModel, cancellationToken);
            return NoContent();
        }

        [HttpPut("update")]
        public async Task<IActionResult> Update(StatisticsContract statisticsContract, CancellationToken cancellationToken)
        {
            var statisticsModel = statisticsContract.Adapt<StatisticsModel>();
            await statisticsService.UpdateAsync(statisticsModel,cancellationToken);
            return NoContent();
        }
    }
}