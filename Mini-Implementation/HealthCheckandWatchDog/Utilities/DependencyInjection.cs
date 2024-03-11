using HealthCheckandWatchDog.Health;

namespace HealthCheckandWatchDog.Utilities;

public static class DependencyInjection
{
    
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {  
        services.AddHealthChecks()
            .AddCheck<DatabaseHealthCheck>("custom-sql",Microsoft.Extensions.Diagnostics.HealthChecks.HealthStatus.Unhealthy);
    }
}
