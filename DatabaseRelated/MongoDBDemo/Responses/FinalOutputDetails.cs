namespace MongoDBDemo.Responses;

public class FinalOutputDetails
{
    public string? Company_Cd { get; set; }
    public string? Company_Nm { get; set; }
    public List<CompanyProductDetails>? companyProducts { get; set; }
}

public class CompanyProductDetails
{
    public string? Product_Nm { get; set; }
    public string? Product_Cd { get; set; }
    public List<ProductUserDetails>? productUsers { get; set; }

}

public class ProductUserDetails
{
    public string? UserName { get; set; }
    public string? Id { get; set; }

}
