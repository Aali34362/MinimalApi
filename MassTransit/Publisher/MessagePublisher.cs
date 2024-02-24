using MassTransit;

namespace MassTransitSample.Publisher;

public class MessagePublisher(IBus bus) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while(!stoppingToken.IsCancellationRequested)
        {
            await bus.Publish<CurrentTime>(
                new
                {
                    Value = $"The currenct time is {DateTime.Now} from MessagePublisher"
                },stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}