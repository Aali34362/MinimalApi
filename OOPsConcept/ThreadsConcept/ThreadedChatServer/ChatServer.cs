using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace ThreadsConcept.ThreadedChatServer;

public class ChatServer
{
    private static TcpListener _server;
    private static ConcurrentDictionary<TcpClient, string> _clients = new ConcurrentDictionary<TcpClient, string>();
    public void HandleClientMain()
    {
        _server = new TcpListener(IPAddress.Loopback, 5000);
        _server.Start();
        Console.WriteLine("Chat server started on local host.");

        while (true)
        {
            TcpClient client = _server.AcceptTcpClient();
            _clients.TryAdd(client, null);
            Thread thread = new Thread(HandleClientFunction!);
            thread.Start(client);
        }

    }
    private void HandleClientFunction(object obj)
    {
        TcpClient client = (TcpClient)obj;
        NetworkStream stream = client.GetStream();
        byte[] buffer = new byte[1024];
        int byteCount;

        Console.WriteLine("Client connected.");

        while ((byteCount = stream.Read(buffer, 0, buffer.Length)) > 0)
        {
            string message = Encoding.ASCII.GetString(buffer, 0, byteCount);
            Console.WriteLine("Received: " + message);
            BroadcastMessage(message, client);
            // Respond back to the sender
            byte[] response = Encoding.ASCII.GetBytes("Server: " + message);
            stream.Write(response, 0, response.Length);
        }

        Console.WriteLine("Client disconnected.");
        _clients.TryRemove(client, out _);
        client.Close();
    }
    private static void BroadcastMessage(string message, TcpClient excludeClient)
    {
        byte[] buffer = Encoding.ASCII.GetBytes("Server: " + message);

        foreach (var client in _clients.Keys)
        {

/*
In the code you provided, the excludeClient parameter is used to prevent sending 
the broadcast message back to the client that sent the message in the first place. 
This is a common pattern in chat servers to avoid echoing a client's message back to itself.
Here's the role of excludeClient and how it works:
Role of excludeClient: The excludeClient parameter represents the client that should 
not receive the broadcast message. This is typically the client that sent the message initially.
Condition if (client != excludeClient): This condition checks if the current client in the loop 
is not the excludeClient. If it is not, then the message is sent to this client. 
If it is the excludeClient, the message is not sent to avoid echoing it back to the sender.
Example Scenario
Suppose there are three clients connected to the server: Client A, Client B, and Client C. 
If Client A sends a message to the server, the server should broadcast this message to Client B and Client C 
but not back to Client A. Here's how it works in your code:
excludeClient would be set to Client A.
The loop iterates over all connected clients.
For each client, it checks if (client != excludeClient).
Since Client A is the excludeClient, it will not receive the message.
Client B and Client C will receive the message because they are not the excludeClient.
*/
            if (client != excludeClient)
            {
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Flush();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error broadcasting message: " + ex.Message);
                }
            }
        }
    }
}
