
using CassandraConnectSample.SessionFactory;

namespace CassandraConnectSample.Repository;

public class Repository<T>(ICassandraSessionFactory sessionFactory) : IRepository<T>
{
    protected readonly ICassandraSessionFactory _sessionFactory = sessionFactory ?? throw new Exception();
    protected readonly ISession _session;

    public Task AddAsync(T entity)
    {
        throw new NotImplementedException();
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<T>> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<T> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity)
    {
        throw new NotImplementedException();
    }
}
