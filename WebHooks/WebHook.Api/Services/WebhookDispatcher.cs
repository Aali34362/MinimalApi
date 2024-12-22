using WebHook.Api.Repositories;

namespace WebHook.Api.Services;

internal sealed class WebhookDispatcher(
    HttpClient httpClient, 
    InMemoryWebHookSubscriptionRepository repository)
{
    public async Task DispatchAsync(string eventType, object payload)
    {
        var subscriptions = repository.GetByEventType(eventType);
        foreach (var item in subscriptions)
        {
            var request = new
            {
                Id = Guid.NewGuid(),
                item.EventType,
                SubscriptionId = item.Id,
                TimeStamp = DateTime.UtcNow,
                Data = payload
            };
            await httpClient.PostAsJsonAsync(item.WebHookUrl, request);
        }
    }
}
