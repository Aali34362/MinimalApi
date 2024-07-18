namespace CassandraConnectSample.UnitOfWork;

public interface IUnitOfWork
{
    IUserRepository Users { get; }
    IOrderRepository Orders { get; }
    IProductRepository Products { get; }
    ICategoryRepository Categories { get; }
    IPaymentRepository Payments { get; }
    IBasketRepository Baskets { get; }
    Task CompleteAsync();
}
