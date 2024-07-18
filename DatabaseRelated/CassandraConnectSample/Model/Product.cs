namespace CassandraConnectSample.Model;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public Guid CategoryId { get; set; }
}
