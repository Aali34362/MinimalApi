namespace MongoDBDemo.Responses;

public class CompanyDetail : BaseResponse
{
    public string? Company_Cd { get; set; }
    public string? Company_Nm { get; set; }
    public string? Company_Dsc { get; set; }
    public List<CompaniesProducts>? companyProducts { get; set; }
}
public class CompaniesProducts
{
    public Guid? ProductId { get; set; }
    public string? Product_Cd { get; set; }
}
