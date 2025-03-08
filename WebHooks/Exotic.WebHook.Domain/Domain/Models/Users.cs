using Exotic.WebHook.Domain.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Exotic.WebHook.Domain.Domain.Models;

[BsonIgnoreExtraElements]
public class User : BaseModel
{
    public string? UserName { get; set; } 
    public string? Email { get; set; } 
}
