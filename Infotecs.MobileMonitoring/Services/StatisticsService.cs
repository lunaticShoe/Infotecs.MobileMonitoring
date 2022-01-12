using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using ILogger = Serilog.ILogger;

namespace Infotecs.MobileMonitoring.Services;

public class StatisticsService : IStatisticsService
{
    //private const string ElementAddedWithId = "Добавлен элемент с id = {0}";
    private readonly IStatisticsRepository _statisticsRepository;
    private readonly ILogger _logger;

    public StatisticsService(IStatisticsRepository statisticsRepository, Serilog.ILogger logger)
    {
        _statisticsRepository = statisticsRepository;
        _logger = logger;
    }
    
    public Task<IEnumerable<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        return _statisticsRepository.GetListAsync(token);
    }

    public async Task CreateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        var existingItem = await _statisticsRepository.Get(statisticsModel.Id, token);

        if (existingItem is not null)
            throw new Exception($"Элемент с id = {statisticsModel.Id} уже существует");
        
        await _statisticsRepository.CreateAsync(statisticsModel, token);
        _logger.Debug($"Добавлен элемент с id = {statisticsModel.Id}");
    }
}