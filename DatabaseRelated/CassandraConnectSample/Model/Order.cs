namespace CassandraConnectSample.Model;

public class Order : BaseEntity
{
    public Guid UserId { get; set; }
    public DateTime OrderDate { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public Payment Payment { get; set; }
}
