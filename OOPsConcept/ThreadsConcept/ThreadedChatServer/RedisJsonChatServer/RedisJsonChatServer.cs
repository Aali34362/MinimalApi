using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Text.Json;

namespace ThreadsConcept.ThreadedChatServer;

public class RedisJsonChatServer
{
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
            _clients.TryAdd(client, null);
            Thread thread = new Thread(HandleRedisJsonClientFunction!);
            thread.Start(client);
        }
    }
    private void HandleRedisJsonClientFunction(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int byteCount;

        Console.WriteLine("Client connected.");

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, byteCount).Trim();
            Console.WriteLine("Received: " + message);
            string response = ProcessMessage(message);
            RedisJsonBroadcastMessage(response, client);
        }

        Console.WriteLine("Client disconnected.");
        _clients.TryRemove(client, out _);
        client.Close();
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
    #region For Simple Data
    private static string JsonHandleCommand(string message)
    {
        string[] parts = message.Split(' ', 3, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 1)
            return "Invalid command";

        string command = parts[0].ToUpper();
        string key = parts.Length > 1 ? parts[1] : null;
        string value = parts.Length > 2 ? parts[2] : null;

        switch (command)
        {
            case "GET":
                return GetJson(key);
            case "POST":
                return PostJson(key, value);
            case "UPDATE":
                return UpdateJson(key, value);
            case "DELETE":
                return DeleteJson(key);
            default:
                return "Unknown command";
        }
    }
    private static string GetJson(string key)
    {
        if (key == null)
            return "Key required";

        if (_dataStore.TryGetValue(key, out string value))
            return $"GET {key}: {value}";
        else
            return $"Key '{key}' not found";
    }
    private static string PostJson(string key, string value)
    {
        if (key == null || value == null)
            return "Key and value required";

        if (_dataStore.TryAdd(key, value))
            return $"POST {key}: {value} added successfully";
        else
            return $"Key '{key}' already exists";
    }
    private static string UpdateJson(string key, string value)
    {
        if (key == null || value == null)
            return "Key and value required";

        if (_dataStore.ContainsKey(key))
        {
            _dataStore[key] = value;
            return $"UPDATE {key}: {value} updated successfully";
        }
        else
            return $"Key '{key}' not found";
    }
    private static string DeleteJson(string key)
    {
        if (key == null)
            return "Key required";

        if (_dataStore.TryRemove(key, out _))
            return $"DELETE {key} removed successfully";
        else
            return $"Key '{key}' not found";
    }
    #endregion
    #region For Json Data
    private static string ProcessMessage(string message)
    {
        try
        {
            var command = JsonSerializer.Deserialize<Command>(message);
            switch (command?.Action?.ToUpper())
            {
                case "GET":
                    return keyValueStore.TryGetValue(command.Key!, out string? value)
                        ? JsonSerializer.Serialize(new { Key = command.Key, Value = value })
                        : JsonSerializer.Serialize(new { Error = $"Key '{command.Key}' not found" });

                case "POST":
                    keyValueStore[command.Key!] = command.Value!;
                    return JsonSerializer.Serialize(new { Message = $"POST {command.Key}: {command.Value} added successfully" });

                case "UPDATE":
                    if (keyValueStore.ContainsKey(command.Key!))
                    {
                        keyValueStore[command.Key!] = command.Value!;
                        return JsonSerializer.Serialize(new { Message = $"UPDATE {command.Key}: {command.Value} updated successfully" });
                    }
                    return JsonSerializer.Serialize(new { Error = $"Key '{command.Key}' not found" });

                case "DELETE":
                    if (keyValueStore.TryRemove(command.Key!, out _))
                    {
                        return JsonSerializer.Serialize(new { Message = $"DELETE {command.Key} removed successfully" });
                    }
                    return JsonSerializer.Serialize(new { Error = $"Key '{command.Key}' not found" });

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

