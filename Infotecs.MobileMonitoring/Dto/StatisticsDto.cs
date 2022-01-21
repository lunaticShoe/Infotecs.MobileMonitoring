using System;
using Infotecs.MobileMonitoring.Models;

namespace Infotecs.MobileMonitoring.Dto;

public class StatisticsDto
{
    public Guid Id { get; set; }
    public string UserName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string ClientVersion { get; set; }
    public string OsName { get; set; }
    public EventModel[] Events { get; set; }
}
