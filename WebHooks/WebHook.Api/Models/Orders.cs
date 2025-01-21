namespace WebHook.Api.Models;

public sealed record Orders(Guid Id, string Customer, decimal Amount, DateTime CreatedAt);
public sealed record CreateOrderRequest(string CustomerName, decimal Amount);

public sealed record WebHookSubscription(Guid Id, string EventType, string WebHookUrl, DateTime CreatedAt);
public sealed record CreateWebHookRequest(string EventType, string WebHookUrl);

public sealed record WebHookEventLog(Guid Id, string EventType, string SubscriptionId, DateTime TimeStamp, string WebHookUrl, string Status, string ResponseMessage);

public record WebHookPayload<T>
{
    public Guid Id { get; set; }
    public string? EventType { get; set; }
    public Guid SubscriptionId { get; set; }
    public DateTime Timestamp { get; set; }
    public T? Data { get; set; }
}

public record WebHookDeliveryAttempt
{
    public Guid Id { get; set; }
    public Guid SubscriptionId { get; set; }
    public string? Payload { get; set; }
    public int ResponseStatusCode { get; set; }
    public bool Success { get; set; }
    public DateTime Timestamp { get; set; }
    public string? DeliveryException { get; set; }
}