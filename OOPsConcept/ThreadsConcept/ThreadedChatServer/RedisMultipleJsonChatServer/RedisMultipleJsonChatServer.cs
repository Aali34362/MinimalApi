using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text.Json;
using System.Text;

namespace ThreadsConcept.ThreadedChatServer;

public class RedisMultipleJsonChatServer
{
    private const int MaxItems = 1000000; // Example limit
    private static TcpListener _server;
    private static ConcurrentDictionary<TcpClient, string> _clients = new ConcurrentDictionary<TcpClient, string>();
    private static ConcurrentDictionary<string, string> _dataStore = new ConcurrentDictionary<string, string>();
    private static readonly ConcurrentDictionary<string, string> keyValueStore = new ConcurrentDictionary<string, string>();

    public void HandleRedisJsonClientMain()
    {
        _server = new TcpListener(IPAddress.Loopback, 5000);
        _server.Start();
        Console.WriteLine("Chat server started on local host.");

        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            //_clients.TryAdd(client, null);
            //Thread thread = new Thread(HandleRedisJsonClientFunction!);
            //thread.Start(client);
            Task.Run(() => HandleRedisJsonClientFunction(client));
        }
    }
    private async Task HandleRedisJsonClientFunction(TcpClient client)
    {
        try
        {
            using NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];
            int byteCount;

            while ((byteCount = await stream.ReadAsync(buffer, 0, buffer.Length)) > 0)
            {
                string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
                Console.WriteLine("Received: " + message);
                string response = ProcessMessage(message);
                ////RedisJsonBroadcastMessage(response,client);
                byte[] responseBuffer = Encoding.ASCII.GetBytes(response);
                await stream.WriteAsync(responseBuffer, 0, responseBuffer.Length);
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error handling client: " + ex.Message);
        }
        finally
        {
            client.Close();
        }
    }
    private static void RedisJsonBroadcastMessage(string message, TcpClient excludeClient)
    {
        byte[] buffer = Encoding.ASCII.GetBytes(message);

        foreach (var client in _clients.Keys)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                stream.Write(buffer, 0, buffer.Length);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error broadcasting message: " + ex.Message);
            }
        }

    }

    #region For Json Data
    private static string ProcessMessage(string message)
    {
        try
        {
            var command = JsonSerializer.Deserialize<DictionaryCommand>(message);
            if (command == null)
            {
                return JsonSerializer.Serialize(new { Error = "Invalid command format" });
            }

            switch (command.Action!.ToUpper())
            {
                case "GET":
                    if (command.KeyValuePairs!.Count != 1)
                    {
                        return JsonSerializer.Serialize(new { Error = "GET requires exactly one key" });
                    }
                    var getResponse = new Dictionary<string, string>();
                    foreach (var key in command.KeyValuePairs.Keys)
                    {
                        getResponse[key] = keyValueStore.TryGetValue(key, out var value) ? value : "Key not found";
                    }
                    return JsonSerializer.Serialize(new { Data = getResponse });

                case "POST":
                    if (keyValueStore.Count + command.KeyValuePairs!.Count > MaxItems)
                    {
                        return JsonSerializer.Serialize(new { Error = "The dictionary has reached its maximum capacity" });
                    }
                    foreach (var kvp in command.KeyValuePairs)
                    {
                        keyValueStore[kvp.Key] = kvp.Value;
                    }
                    return JsonSerializer.Serialize(new { Message = "POST operation completed successfully" });

                case "UPDATE":
                    var updateResponse = new Dictionary<string, string>();
                    foreach (var kvp in command.KeyValuePairs!)
                    {
                        if (keyValueStore.ContainsKey(kvp.Key))
                        {
                            keyValueStore[kvp.Key] = kvp.Value;
                            updateResponse[kvp.Key] = "Updated";
                        }
                        else
                        {
                            updateResponse[kvp.Key] = "Key not found";
                        }
                    }
                    return JsonSerializer.Serialize(new { Message = updateResponse });

                case "DELETE":
                    if (command.KeyValuePairs!.Count != 1)
                    {
                        return JsonSerializer.Serialize(new { Error = "DELETE requires exactly one key" });
                    }
                    var deleteResponse = new Dictionary<string, string>();
                    foreach (var key in command.KeyValuePairs.Keys)
                    {
                        if (keyValueStore.TryRemove(key, out _))
                        {
                            deleteResponse[key] = "Deleted";
                        }
                        else
                        {
                            deleteResponse[key] = "Key not found";
                        }
                    }
                    return JsonSerializer.Serialize(new { Message = deleteResponse });

                case "DELETEALL":
                    keyValueStore.Clear();
                    return JsonSerializer.Serialize(new { Message = "All keys removed successfully" });

                case "GETALL":
                    return JsonSerializer.Serialize(new { Data = keyValueStore });

                default:
                    return JsonSerializer.Serialize(new { Error = "Unknown command" });
            }
        }
        catch (Exception ex)
        {
            return JsonSerializer.Serialize(new { Error = $"Failed to process message: {ex.Message}" });
        }
    }
    #endregion
}
