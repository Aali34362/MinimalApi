namespace WebHook.Api.Models;

public sealed record Orders(Guid Id, string Customer, decimal Amount, DateTime CreatedAt);
public sealed record CreateOrderRequest(string CustomerName, decimal Amount);

public sealed record WebHookSubscription(Guid Id, string EventType, string WebHookUrl, DateTime CreatedAt);
public sealed record CreateWebHookRequest(string EventType, string WebHookUrl);