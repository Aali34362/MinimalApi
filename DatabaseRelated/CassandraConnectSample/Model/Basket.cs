namespace CassandraConnectSample.Model;

public class Basket : BaseEntity
{
    public Guid UserId { get; set; }
    public List<BasketItem> BasketItems { get; set; } = new List<BasketItem>();
}
