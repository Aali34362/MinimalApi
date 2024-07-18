namespace CassandraConnectSample.Model;

public class BasketItem : BaseEntity
{
    public Guid BasketId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
}
