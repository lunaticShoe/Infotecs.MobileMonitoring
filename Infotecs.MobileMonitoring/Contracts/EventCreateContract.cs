namespace Infotecs.MobileMonitoring.Contracts;

public class EventCreateContract
{
    public string Name { get; set; }
    public Guid StatisticsId { get; set; }
    public DateTime CreatedAt { get; set; }
}
