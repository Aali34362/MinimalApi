using Dapper;
using HealthCheckandWatchDog.Database;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Data;

namespace HealthCheckandWatchDog.Health;

internal sealed class DatabaseHealthCheck(DBConnectionFactory dbConnection) : IHealthCheck
{
    public async Task<HealthCheckResult> CheckHealthAsync(
        HealthCheckContext context, 
        CancellationToken cancellationToken = default)
    {
        try
        {
            using IDbConnection connection = dbConnection.OpenConnection();
            var result = await connection.ExecuteScalarAsync<int>("Select 1");
            if (result == 1)
            {
                return HealthCheckResult.Healthy();
            }
            else
            {
                return HealthCheckResult.Unhealthy("Query result does not match expected value.");
            }
        }
        catch(Exception  ex)
        {
            return HealthCheckResult.Unhealthy(ex.Message);
        }
    }
}
