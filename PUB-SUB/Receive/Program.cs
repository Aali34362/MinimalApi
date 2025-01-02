using RabbitMQ.Client.Events;
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

    Console.WriteLine(" [*] Waiting for messages.");

    var consumer = new AsyncEventingBasicConsumer(channel);
    consumer.ReceivedAsync += (model, ea) =>
    {
        var body = ea.Body.ToArray();
        var message = Encoding.UTF8.GetString(body);
        Console.WriteLine($" [x] Received {message}");
        return Task.CompletedTask;
    };

    await channel.BasicConsumeAsync($"hello{i}", autoAck: true, consumer: consumer);
}