namespace CassandraConnectSample.SessionFactory;

public interface ICassandraSessionFactory
{
    ISession GetSession();
}
