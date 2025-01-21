using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Channels;
using WebHook.Api.Data;
using WebHook.Api.Models;
using WebHook.Api.Repositories;
using WebHooks.OpenTelemetry;

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

internal sealed class WebHookProcessor(IServiceScopeFactory scopeFactory, Channel<WebHookDispatch> webhooksChannel) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (WebHookDispatch dispatch in webhooksChannel.Reader.ReadAllAsync(stoppingToken))
        {
            using Activity? activity = DiagnosticConfig.source.StartActivity($"{dispatch.EventType} process webhook", ActivityKind.Internal, parentId: dispatch.ParentActivityId);

            using IServiceScope scope = scopeFactory.CreateScope();
            var dispatcher = scope.ServiceProvider.GetRequiredService<WebhookDBDispatcher>();
            await dispatcher.ProcessAsync(dispatch.EventType, dispatch.Data);
        }
    }
}

internal sealed record WebHookDispatch(string EventType, object Data, string? ParentActivityId);

internal sealed class WebhookDBDispatcher(
    Channel<WebHookDispatch> webhooksChannel,
    IHttpClientFactory httpClientFactory,
    WebHooksDbContext repository)
{
    public async Task ProcessAsync<T>(string eventType, T data) where T : notnull
    {
        using Activity? activity = DiagnosticConfig.source.StartActivity($"{eventType} dispatch webhook");
        activity?.AddTag("event.type", eventType);
        await webhooksChannel.Writer.WriteAsync(new WebHookDispatch(eventType, data, activity?.Id));
    }

    public async Task DispatchAsync<T>(string eventType, T data)
    {
        var subscriptions = await repository.Subscriptions.AsNoTracking().Where(s => s.EventType == eventType).ToListAsync();

        foreach (var item in subscriptions)
        {
            var httpClient = httpClientFactory.CreateClient();
            var payload = new WebHookPayload<T>
            {
                Id = Guid.NewGuid(),
                EventType = item.EventType,
                SubscriptionId = item.Id,
                Timestamp = DateTime.Now,
                Data = data
            };

            var jsonPayload = JsonSerializer.Serialize(payload);

            try
            {
                var response = await httpClient.PostAsJsonAsync(item.WebHookUrl, payload);
                var attempt = new WebHookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    Payload = jsonPayload,
                    ResponseStatusCode = (int)response.StatusCode,
                    Success = response.IsSuccessStatusCode,
                    Timestamp = DateTime.Now,
                    SubscriptionId = item.Id,
                };
                repository.DeliveryAttempt.Add(attempt);
                await repository.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                var attempt = new WebHookDeliveryAttempt
                {
                    Id = Guid.NewGuid(),
                    Payload = jsonPayload,
                    ResponseStatusCode = 0,
                    Success = false,
                    Timestamp = DateTime.Now,
                    SubscriptionId = item.Id,
                    DeliveryException = ex.Message,
                };
                repository.DeliveryAttempt.Add(attempt);
                await repository.SaveChangesAsync();

                Console.WriteLine(ex);
                throw;
            }
        }
    }
}