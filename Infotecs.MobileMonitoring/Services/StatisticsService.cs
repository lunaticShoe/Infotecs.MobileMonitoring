using Infotecs.MobileMonitoring.Dto;
using Infotecs.MobileMonitoring.Exceptions;
using Infotecs.MobileMonitoring.Interfaces;
using Infotecs.MobileMonitoring.Models;
using Mapster;
using ILogger = Serilog.ILogger;

namespace Infotecs.MobileMonitoring.Services;

public class StatisticsService : IStatisticsService
{
    //private const string ElementAddedWithId = "Добавлен элемент с id = {0}";
    //private readonly IStatisticsRepository statisticsRepository;
    //private readonly IUnitOfWorkFactory unitOfWorkFactory;
    private readonly IUnitOfWork unitOfWork;
    private readonly ISessionContainerFactory sessionContainerFactory;
    private readonly IEventService eventService;
    private readonly ILogger logger;

    public StatisticsService(IUnitOfWork unitOfWork, ISessionContainerFactory sessionContainerFactory, 
        IEventService eventService, ILogger logger)
    {
        //this.statisticsRepository = statisticsRepository;
        //this.unitOfWorkFactory = unitOfWorkFactory;
        this.unitOfWork = unitOfWork;
        this.sessionContainerFactory = sessionContainerFactory;
        this.eventService = eventService;
        this.logger = logger;
    }
    
    public Task<ICollection<StatisticsModel>> GetListAsync(CancellationToken token = default)
    {
        return unitOfWork.StatisticsRepository.GetListAsync(token);
    }

    public Task<StatisticsModel> GetAsync(Guid id, CancellationToken token = default)
    {
        return unitOfWork.StatisticsRepository.GetAsync(id, token);
    }
    
    public async Task CreateAsync(StatisticsDto statisticsDto, CancellationToken token = default)
    {
        var existingItem = await unitOfWork.StatisticsRepository.GetAsync(statisticsDto.Id, token);

        if (existingItem is not null)
            throw new ElementAlreadyExistsException(statisticsDto.Id);
        
        statisticsDto.CreatedAt = DateTime.UtcNow;

        using (var session = sessionContainerFactory.Create(unitOfWork))
        {
            await unitOfWork.StatisticsRepository
                .CreateAsync(statisticsDto.Adapt<StatisticsModel>(), token);
            await eventService
                .CreateRangeAsync(statisticsDto.Id, statisticsDto.Events, token);
            // await unitOfWork.EventRepository
            //     .CreateRangeAsync(statisticsDto.Events, token);
            await session.SaveAsync(token);
        }
        
        // await unitOfWork.ExecuteTransactionAsync(async unitOfWork =>
        // {
        //     await unitOfWork.StatisticsRepository
        //         .CreateAsync(statisticsDto.Adapt<StatisticsModel>(), token);
        //     await unitOfWork.EventRepository
        //         .CreateRangeAsync(statisticsDto.Events, token);
        //     await unitOfWork.SaveAsync();
        // }, token);
        
        //await unitOfWork.StatisticsRepository.CreateAsync(statisticsDto, token);
        logger.Debug("Element added: {@Statistics}",statisticsDto);
    }

    public async Task UpdateAsync(StatisticsModel statisticsModel, CancellationToken token = default)
    {
        var existingItem = await unitOfWork.StatisticsRepository.GetAsync(statisticsModel.Id, token);

        if (existingItem is null)
            throw new Exception($"Element with id = {statisticsModel.Id} does not exists");
        await unitOfWork.StatisticsRepository.UpdateAsync(statisticsModel, token);
        logger.Debug(
            "Element altered from {@StatisticsOld}, to {@StatisticsNew}", 
            existingItem,statisticsModel);
      
        
    } 
}