using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using ILogger = Serilog.ILogger;

namespace Infotecs.MobileMonitoring.Services;

public class StatisticsService : IStatisticsService
{
    //private const string ElementAddedWithId = "Добавлен элемент с id = {0}";
    private readonly IStatisticsRepository statisticsRepository;
    private readonly ILogger logger;

    public StatisticsService(IStatisticsRepository statisticsRepository, Serilog.ILogger logger)
    {
        this.statisticsRepository = statisticsRepository;
        this.logger = logger;
    }
    
    public Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        return statisticsRepository.GetListAsync(token);
    }

    public async Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        var existingItem = await statisticsRepository.GetAsync(statisticsModel.Id, token);

        if (existingItem is not null)
            throw new Exception($"Element with id = {statisticsModel.Id} already exists");
        
        await statisticsRepository.CreateAsync(statisticsModel, token);
        logger.Debug("Element added: {@Statistics}",statisticsModel);
    }

    public async Task UpdateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        var existingItem = await statisticsRepository.GetAsync(statisticsModel.Id, token);

        if (existingItem is null)
            throw new Exception($"Element with id = {statisticsModel.Id} does not exists");
        await statisticsRepository.UpdateAsync(existingItem, statisticsModel, token);
        logger.Debug(
            "Element altered from {@StatisticsOld}, to {@StatisticsNew}", 
            existingItem,statisticsModel);
      
        
    } 
}