using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;

namespace Receive;

public static class ConsumerWorkQueues
{
    public static async Task ConsumerWorkQueues_Main()
    {
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
            //consumer.ReceivedAsync += async (model, ea) =>
            //{
            //    Console.WriteLine($" [x] Received {message}");

            //    int dots = message.Split('.').Length - 1;
            //    await Task.Delay(dots * 1000);

            //    Console.WriteLine(" [x] Done");
            //};

            await channel.BasicConsumeAsync($"hello{i}", autoAck: true, consumer: consumer);
        }
    }
}
