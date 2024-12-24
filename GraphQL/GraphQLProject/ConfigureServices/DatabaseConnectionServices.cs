using GraphQLProject.DatabaseConnections.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GraphQLProject.ConfigureServices;

public static class DatabaseConnectionServices
{
    public static void AddDbConnectionServices(this IServiceCollection serviceDescriptors, IConfiguration configuration)
    {
        ////serviceDescriptors.AddDbContext<GraphqlDbContext>(options =>
        ////    options.UseSqlServer(
        ////        ////configuration.GetConnectionString("DBConnection"), // Use the configuration object here
        ////        ////sqlOptions => sqlOptions.EnableRetryOnFailure()
        ////    )
        ////    .UseLazyLoadingProxies()
        ////    .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
        ////    .LogTo(Console.WriteLine, LogLevel.Information) // Log information
        ////    .EnableSensitiveDataLogging() // Debugging only
        ////    .EnableDetailedErrors()); // Detailed errors

        serviceDescriptors.AddDbContext<GraphqlDbContext>((serviceProvider, options) =>
        {
            var configuration = serviceProvider.GetRequiredService<IConfiguration>();
            var connectionString = configuration.GetConnectionString("DBConnection");

            if (!string.IsNullOrEmpty(connectionString))
            {
                options.UseSqlServer(connectionString)
                       .UseLazyLoadingProxies()
                       .UseQueryTrackingBehavior(QueryTrackingBehavior.TrackAll)
                       .LogTo(Console.WriteLine, LogLevel.Information)
                       .EnableSensitiveDataLogging()
                       .EnableDetailedErrors();
            }
            else
            {
                throw new InvalidOperationException("Database connection string is missing.");
            }
        });
    }
}
