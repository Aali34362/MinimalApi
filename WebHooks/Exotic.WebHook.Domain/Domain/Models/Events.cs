using Exotic.WebHook.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;

namespace Exotic.WebHook.Domain.Domain.Models;

[BsonIgnoreExtraElements]
public class Events : BaseModel
{
    public string? Event { get; set; }
    public string? EventDescription { get; set; }
    public List<EventTypes>? EventTypes { get; set; }    
}
[BsonIgnoreExtraElements]
public class EventTypes
{
    public string? EventType { get; set; }
}