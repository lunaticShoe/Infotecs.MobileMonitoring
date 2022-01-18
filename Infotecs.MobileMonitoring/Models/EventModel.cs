using MongoDB.Bson;

namespace Infotecs.MobileMonitoring.Models;

public class EventModel
{
    public ObjectId Id { get; set; }
    public string Name { get; set; }
    public DateTime CreatedAt { get; set; }
    public Guid StatisticsId { get; set; }
}
