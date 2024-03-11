using System.Data;
using System.Data.SqlClient;

namespace HealthCheckandWatchDog.Database;

public class SqlConnectionFactory : IDbConnectionFactory
{
    private readonly string connectionString;

    public SqlConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDbConnection OpenConnection()
    {
        SqlConnection connection = new SqlConnection(connectionString);
        connection.Open();
        return connection;
    }
}