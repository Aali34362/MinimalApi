using Exotic.WebHook.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Exotic.WebHook.Domain.Domain.Models;

[BsonIgnoreExtraElements]
public class Subscriptions : BaseModel
{
    [BsonRepresentation(BsonType.String)]
    public Guid UserPool_Id { get; set; }
    public string? WebHookUrl { get; set; }
    public List<EventPool>? EventPool { get; set; }
}

[BsonIgnoreExtraElements]
public class EventPool
{
    [BsonRepresentation(BsonType.String)]
    public Guid Event_Id { get; set; }
    public string? Event { get; set; }
    public List<EventTypes>? EventTypes { get; set; }
}