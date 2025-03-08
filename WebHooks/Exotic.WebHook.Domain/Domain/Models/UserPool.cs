using Exotic.WebHook.Domain.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace Exotic.WebHook.Domain.Domain.Models;

[BsonIgnoreExtraElements]
public class UserPool : BaseModel
{
    [BsonRepresentation(BsonType.String)]
    public Guid User_Id { get; set; }
    public string? PoolName { get; set; }
    public List<PoolUser> PoolUsers { get; set; } = [];
}

[BsonIgnoreExtraElements]
public class PoolUser
{
    public string? UserName { get; set; }
    public string? Email { get; set; }
}