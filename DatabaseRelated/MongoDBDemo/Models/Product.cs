namespace MongoDBDemo.Models;

public class Product : BaseEntity
{
    public string? Product_Cd { get; set; }
    public string? Product_Nm { get; set; }
    public string? Product_Dsc { get; set; }
    [BsonIgnoreIfNull]
    public List<ProductUser>? productUsers { get; set; }
}
public class ProductUser
{
    public Guid UserId { get; set; }
    public string? UserName { get; set; }
}
