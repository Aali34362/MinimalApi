using WebHook.Api.Models;

namespace WebHook.Api.Repositories;

internal sealed class InMemoryOrderRepository
{
    private readonly List<Orders> _orders = [];
    public void Add(Orders order) => _orders.Add(order);
    public IReadOnlyList<Orders> GetAll() => _orders.AsReadOnly();    
}

internal sealed class InMemoryWebHookSubscriptionRepository
{
    private readonly List<WebHookSubscription> _webHooks = [];
    public void Add(WebHookSubscription webHook) => _webHooks.Add(webHook);
    public IReadOnlyList<WebHookSubscription> GetByEventType(string eventType) => _webHooks.Where(w=>w.EventType == eventType).ToList().AsReadOnly();
}

internal sealed class InMemoryEventLogRepository
{
    private readonly List<WebHookEventLog> _eventLogs = new();
    public void Add(WebHookEventLog log) => _eventLogs.Add(log);
    public IReadOnlyList<WebHookEventLog> GetAll() => _eventLogs.AsReadOnly();
}