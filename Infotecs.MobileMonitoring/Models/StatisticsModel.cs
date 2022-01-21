using System;

namespace Infotecs.MobileMonitoring.Models;


public class StatisticsModel
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ClientVersion { get; set; }
    public string OsName { get; set; }
}