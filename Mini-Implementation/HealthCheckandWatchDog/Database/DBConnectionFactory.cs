using System.Data;
using System.Data.SqlClient;
namespace HealthCheckandWatchDog.Database;

public class DBConnectionFactory
{
    private readonly string connectionString;

    public DBConnectionFactory(string connectionString)
    {
        this.connectionString = connectionString;
    }

    public IDbConnection OpenConnection()
    {
        // Implement the logic to create and open a database connection
        // This is a simplified example; replace it with your actual database logic
        IDbConnection connection = new SqlConnection(connectionString);
        //IDbConnection connection = new SqlConnection("Server=UCHIHA_MADARA\\SQLEXPRESS;Database=SocietyManagementSystem;User=UCHIHA_MADARA\\aa882;Password=; MultipleActiveResultSets=False;Encrypt=False;TrustServerCertificate=true;Connection Timeout=6000;Trusted_Connection=True;");
        connection.Open();
        return connection;
    }
}
