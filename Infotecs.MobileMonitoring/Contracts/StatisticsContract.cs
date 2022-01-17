using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Contracts;

public class StatisticsContract
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public string ClientVersion { get; set; }
    public string OsName { get; set; }
    public ICollection<EventContract> Events { get; set; }
}
