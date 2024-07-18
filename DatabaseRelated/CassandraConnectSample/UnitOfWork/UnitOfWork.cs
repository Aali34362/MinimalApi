using CassandraConnectSample.SessionFactory;

namespace CassandraConnectSample.UnitOfWork;

public class UnitOfWork(ICassandraSessionFactory sessionFactory) : IUnitOfWork
{
    private readonly ICassandraSessionFactory _sessionFactory = sessionFactory;
    private UserRepository _users;
    private OrderRepository _orders;
    private ProductRepository _products;
    private CategoryRepository _categories;
    private PaymentRepository _payments;
    private BasketRepository _baskets;

    public IUserRepository Users => _users ??= new UserRepository(_sessionFactory);
    public IOrderRepository Orders => _orders ??= new OrderRepository(_sessionFactory);
    public IProductRepository Products => _products ??= new ProductRepository(_sessionFactory);
    public ICategoryRepository Categories => _categories ??= new CategoryRepository(_sessionFactory);
    public IPaymentRepository Payments => _payments ??= new PaymentRepository(_sessionFactory);
    public IBasketRepository Baskets => _baskets ??= new BasketRepository(_sessionFactory);

    public async Task CompleteAsync()
    {
        // In Cassandra, there's no direct equivalent of committing a transaction like in SQL databases.
        // However, we can ensure all changes are applied consistently by implementing each repository method correctly.
    }
}
