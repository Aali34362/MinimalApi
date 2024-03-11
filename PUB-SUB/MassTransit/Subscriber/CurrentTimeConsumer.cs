using MassTransit;
using MassTransitSample.Publisher;

namespace MassTransitSample.Subscriber;

public class CurrentTimeConsumer(ILogger<CurrentTimeConsumer> _logger) : IConsumer<CurrentTime>
{
    public Task Consume(ConsumeContext<CurrentTime> context)
    {
        _logger.LogInformation("{Consumer} : {Message}", nameof(CurrentTimeConsumer), context.Message.Value);
        return Task.CompletedTask;
    }
}

public class CurrentTimeConsumerV2(ILogger<CurrentTimeConsumerV2> _logger) : IConsumer<CurrentTime>
{
    public Task Consume(ConsumeContext<CurrentTime> context)
    {
        _logger.LogInformation("{Consumer} : {Message}", nameof(CurrentTimeConsumerV2), context.Message.Value);
        return Task.CompletedTask;
    }
}

