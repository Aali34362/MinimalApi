using System.Reflection;
using System.Text.Json;

namespace OcelotApiGateway;

public static class ConfigurationExtensions
{
    public static IConfigurationBuilder AddOcelotConfigs(this IConfigurationBuilder builder)
    {
        FileInfo _dataRoot = new FileInfo(Assembly.GetExecutingAssembly().Location);
        string assemblyFolderPath = _dataRoot.Directory!.Parent!.Parent!.Parent!.FullName;
        Console.WriteLine($"Path : {assemblyFolderPath}");

        var routes = new List<object>();
        
        var createOrderConfig = File.ReadAllText("ocelot.Checkout.json");
        var createOrderData = JsonSerializer.Deserialize<Dictionary<string, object>>(createOrderConfig);
        if (createOrderData != null && createOrderData.ContainsKey("Routes"))
        {
            routes.AddRange(JsonSerializer.Deserialize<List<object>>(createOrderData["Routes"].ToString()!)!);
        }

        var processOrderConfig = File.ReadAllText("ocelot.Orders.json");
        var processOrderData = JsonSerializer.Deserialize<Dictionary<string, object>>(processOrderConfig);
        if (processOrderData != null && processOrderData.ContainsKey("Routes"))
        {
            routes.AddRange(JsonSerializer.Deserialize<List<object>>(processOrderData["Routes"].ToString()!)!);
        }

        // Create a combined configuration object
        var combinedConfig = new Dictionary<string, object>
        {
            ["Routes"] = routes,
            ["GlobalConfiguration"] = new
            {
                BaseUrl = "https://localhost:60884"
            }
        };

        var json = JsonSerializer.Serialize(combinedConfig);
        File.WriteAllText($"{assemblyFolderPath}/ocelot.json", json);

        return builder.AddJsonFile("ocelot.json", optional: true, reloadOnChange: true);
    }
}
