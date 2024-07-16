namespace MongoDBDemo.Models;

public class Company : BaseEntity
{
    public string? Company_Cd { get; set; }
    public string? Company_Nm { get; set; }
    public string? Company_Dsc { get; set; }
    [BsonIgnoreIfNull]
    public List<CompanyProduct>? companyProducts { get; set; }
}
public class CompanyProduct
{
    public Guid? ProductId { get; set; }
    public string? Product_Cd { get; set; }
}
