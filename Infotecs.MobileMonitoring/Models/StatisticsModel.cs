namespace Infotecs.MobileMonitoring.Models;

public enum DeviceType
{
    Android,
    Ios,
}

public class StatisticsModel
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ClientVersion { get; set; }
    public DeviceType DeviceType { get; set; }
}