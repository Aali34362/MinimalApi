using Microsoft.Extensions.Configuration;

namespace CassandraConnectSample.SessionFactory;

public class CassandraSessionFactory : ICassandraSessionFactory
{
    private readonly Cluster _cluster;
    private readonly ISession _session;

    public CassandraSessionFactory(IConfiguration configuration)
    {
        var contactPoints = configuration.GetSection("Cassandra:ContactPoints").Get<string[]>();
        var keyspace = configuration.GetValue<string>("Cassandra:Keyspace");

        _cluster = Cluster.Builder()
                          .AddContactPoints(contactPoints)
                          .Build();

        _session = _cluster.Connect(keyspace);

        // Create keyspace and tables if they do not exist
        _session.Execute("CREATE KEYSPACE IF NOT EXISTS ecommerce WITH REPLICATION = {'class': 'SimpleStrategy', 'replication_factor': 1}");
        _session.Execute("USE ecommerce");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS users (
                id uuid PRIMARY KEY,
                username text,
                email text,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS orders (
                id uuid PRIMARY KEY,
                userid uuid,
                orderdate timestamp,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS orderitems (
                id uuid PRIMARY KEY,
                orderid uuid,
                productid uuid,
                quantity int,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS products (
                id uuid PRIMARY KEY,
                name text,
                categoryid uuid,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS categories (
                id uuid PRIMARY KEY,
                name text,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS payments (
                id uuid PRIMARY KEY,
                orderid uuid,
                amount decimal,
                paymentdate timestamp,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS baskets (
                id uuid PRIMARY KEY,
                userid uuid,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute(@"
            CREATE TABLE IF NOT EXISTS basketitems (
                id uuid PRIMARY KEY,
                basketid uuid,
                productid uuid,
                quantity int,
                crtd_usr text,
                crtd_dt timestamp,
                lst_crtd_usr text,
                lst_crtd_dt timestamp,
                actv_ind smallint,
                del_ind smallint
            )");

        _session.Execute("CREATE INDEX IF NOT EXISTS user_email_idx ON users (email)");
        ////_cluster.Dispose();
    }

    public ISession GetSession()
    {
        return _session;
    }
}
