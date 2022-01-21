using MongoDB.Bson;

namespace Infotecs.MobileMonitoring.Models;

public class EventDictionaryModel
{
    public ObjectId Id { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
}
