namespace MongoDBDemo.Responses;

public class ProductDetail : BaseResponse
{
    public string? Product_Cd { get; set; }
    public string? Product_Nm { get; set; }
    public string? Product_Dsc { get; set; }
    public List<ProductUsers>? productUsers { get; set; }
}
public class ProductUsers
{
    public Guid? UserId { get; set; }
    public string? UserName { get; set; }
}
