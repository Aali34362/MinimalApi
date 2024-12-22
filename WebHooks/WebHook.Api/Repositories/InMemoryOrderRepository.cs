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