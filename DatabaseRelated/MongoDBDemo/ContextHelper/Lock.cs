namespace MongoDBDemo.ContextHelper;

public class Lock
{
    public Guid? Id { get; set; }  // user Id
    public string? Resource { get; set; } // e.g., "Company_12345"
    public string? Operation { get; set; } // "read" or "write"
    public DateTime CreatedAt { get; set; }
}
