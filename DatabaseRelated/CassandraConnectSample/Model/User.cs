namespace CassandraConnectSample.Model;

public class User : BaseEntity
{
    public string Username { get; set; }
    public string Email { get; set; }
    public List<Order> Orders { get; set; } = new List<Order>();
}
