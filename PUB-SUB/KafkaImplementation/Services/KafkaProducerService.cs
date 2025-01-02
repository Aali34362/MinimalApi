using Confluent.Kafka;

namespace KafkaImplementation.Services;

public class KafkaProducerService(IConfiguration configuration)
{
    private readonly string _bootstrapServers = configuration["Kafka:BootstrapServers"]!;
    private readonly string _topic = configuration["Kafka:Topic"]!;

    public async Task ProduceAsync(string key, string message)
    {
        var config = new ProducerConfig { BootstrapServers = _bootstrapServers };

        using var producer = new ProducerBuilder<string, string>(config).Build();
        try
        {
            var deliveryResult = await producer.ProduceAsync(_topic, new Message<string, string>
            {
                Key = key,
                Value = message
            });

            Console.WriteLine($"Message '{message}' sent to '{deliveryResult.TopicPartitionOffset}'");
        }
        catch (ProduceException<string, string> ex)
        {
            Console.WriteLine($"Delivery failed: {ex.Error.Reason}");
        }
    }
}
