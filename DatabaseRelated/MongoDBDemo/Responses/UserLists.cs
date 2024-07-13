namespace MongoDBDemo.Responses;

public class UserLists : BaseResponse
{
    public string? DisplayName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }
    public string? UserEmail { get; set; }
    public string? UserPhone { get; set; }
    public string? address { get; set; }
}
