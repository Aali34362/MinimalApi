using System.Collections.Concurrent;
using System.Net.Sockets;
using System.Net;
using System.Text;

namespace ThreadsConcept.ThreadedChatServer;

public class RedisChatServer
{
    private static TcpListener _server;
    private static ConcurrentDictionary<TcpClient, string> _clients = new ConcurrentDictionary<TcpClient, string>();
    private static ConcurrentDictionary<string, string> _dataStore = new ConcurrentDictionary<string, string>();
    public void HandleRedisClientMain()
    {
        _server = new TcpListener(IPAddress.Loopback, 5000);
        _server.Start();
        Console.WriteLine("Chat server started on local host.");

        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            _clients.TryAdd(client, null);
            Thread thread = new Thread(HandleRedisClientFunction!);
            thread.Start(client);
        }
    }
    private void HandleRedisClientFunction(object obj)
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
            string response = HandleCommand(message);
            RedisBroadcastMessage(response, client);
        }

        Console.WriteLine("Client disconnected.");
        _clients.TryRemove(client, out _);
        client.Close();
    }
    private static void RedisBroadcastMessage(string message, TcpClient excludeClient)
    {
        byte[] buffer = Encoding.ASCII.GetBytes("Server: " + message);

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
    private static string HandleCommand(string message)
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
                return Get(key);
            case "POST":
                return Post(key, value);
            case "UPDATE":
                return Update(key, value);
            case "DELETE":
                return Delete(key);
            default:
                return "Unknown command";
        }
    }
    private static string Get(string key)
    {
        if (key == null)
            return "Key required";

        if (_dataStore.TryGetValue(key, out string value))
            return $"GET {key}: {value}";
        else
            return $"Key '{key}' not found";
    }
    private static string Post(string key, string value)
    {
        if (key == null || value == null)
            return "Key and value required";

        if (_dataStore.TryAdd(key, value))
            return $"POST {key}: {value} added successfully";
        else
            return $"Key '{key}' already exists";
    }
    private static string Update(string key, string value)
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
    private static string Delete(string key)
    {
        if (key == null)
            return "Key required";

        if (_dataStore.TryRemove(key, out _))
            return $"DELETE {key} removed successfully";
        else
            return $"Key '{key}' not found";
    }
}

