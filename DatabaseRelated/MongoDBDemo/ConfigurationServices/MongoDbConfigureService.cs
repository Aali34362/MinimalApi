using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MongoDBDemo.ConfigurationServices;

public static class MongoDbConfigureService
{
    public static void AddMongoDBDocumentStore<T>(this IServiceCollection services)
    {
        services.AddSingleton<IDocumentWrapper<T>>(provider =>
        {
            ////var configuration = provider.GetRequiredService<IConfiguration>();
            ////var connectionString = configuration["MongoDBSettings:ConnectionString"]!;
            ////var databaseName = configuration["MongoDBSettings:DatabaseName"]!;
            ////return new DocumentWrapper<T>(connectionString, databaseName);

            var mongoDbConfig = provider.GetRequiredService<MongoDbConfiguration>();
            return new DocumentWrapper<T>(mongoDbConfig.ConnectionString!, mongoDbConfig.DatabaseName!);
        });
    }
    public static void AddMongoDBServices(this IServiceCollection services)
    {
        services.AddMongoDBDocumentStore<User>();
        services.AddMongoDBDocumentStore<Product>();
        services.AddMongoDBDocumentStore<Company>();
    }
}
