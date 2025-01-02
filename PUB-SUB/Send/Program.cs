using RabbitMQ.Client;
using System.Text;

for (int i = 0; i < 10; i++)
{
    var factory = new ConnectionFactory { HostName = "localhost" };
    using var connection = await factory.CreateConnectionAsync();
    using var channel = await connection.CreateChannelAsync();

    await channel.QueueDeclareAsync(
        queue: $"hello{i}", 
        durable: false, 
        exclusive: false, 
        autoDelete: false,
        arguments: null);

    string message = $"Hello World! : {i}";
    var body = Encoding.UTF8.GetBytes(message);

    await channel.BasicPublishAsync(
        exchange: string.Empty, 
        routingKey: $"hello{i}", 
        body: body);
    Console.WriteLine($" [x] Sent {message}");    
}
