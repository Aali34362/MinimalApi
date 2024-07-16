using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using MongoDBDemo.ConfigurationServices;
using MongoDBDemo.MainOperations;

public class Program
{
    private static void Main(string[] args) 
    {
        var host = CreateHostBuilder(args).Build();

        // Resolve the App service and call its Run method
        var app = host.Services.GetRequiredService<App>();
        app.Run();

        host.Run();
    }
    public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((hostingContext, config) =>
                {
                    var env = hostingContext.HostingEnvironment;
                    config.SetBasePath(env.ContentRootPath)
                          .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                          .AddEnvironmentVariables();
                })
                .ConfigureServices((context, services) =>
                {
                    services.AddSingleton<App>();
                    services.AddSingleton<UserOperations>();
                    services.AddTransient<IMongoRepository, MongoRepository>();
                    services.AddTransient<ILockService, LockService>();
                    services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
                    var mongoDbConfig = context.Configuration.GetSection("MongoDbConfiguration").Get<MongoDbConfiguration>();
                    services.AddSingleton(mongoDbConfig);

                    services.AddMongoDBServices();
                });
}
public class App(ILogger<App> logger, UserOperations userOperations, MongoDbConfiguration mongoDbConfig)
{
    private readonly ILogger<App> _logger = logger;
    private readonly UserOperations _userOperations = userOperations;
    private readonly MongoDbConfiguration _mongoDbConfig = mongoDbConfig;

    public async void Run()
    {
        // Application logic here
        _logger.LogInformation("Console app running with MongoDB connection string: {ConnectionString}", _mongoDbConfig.ConnectionString);
        Console.WriteLine("Console app running...");

        // Example usage of UserOperations
        ////await _userOperations.CreateUser();
        ///await _userOperations.GetUserList();
        //await _userOperations.GetUserById();
        ////await _userOperations.GetUserByName();
        ////await _userOperations.SoftDeleteUser();
        ////await _userOperations.DeleteUser();
        ////_userOperations.DynamicJsonClass();
        ///
        // await _userOperations.CreateReadLock();
        await _userOperations.DisposeReadLock();
        await _userOperations.GetReadReadLock();
        await _userOperations.GetReadWriteLock();


        //await _userOperations.CreateWriteLock();
        await _userOperations.DisposeWriteLock();
        await _userOperations.GetReadWriteLock2();
        await _userOperations.GetWriteWriteLock();
        
    }
}