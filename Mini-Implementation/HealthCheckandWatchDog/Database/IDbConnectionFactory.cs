using System.Data;

namespace HealthCheckandWatchDog.Database;

public interface IDbConnectionFactory
{
    IDbConnection OpenConnection();
}
