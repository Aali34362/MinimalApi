using Microsoft.AspNetCore.SignalR;
using WebHook.Receiver.Api.Models;

namespace WebHook.Receiver.Api.HubService;

public class WebhookHub : Hub
{
    public async Task NotifyNewWebhook(string uniqueId, WebhookData data)
    {
        await Clients.Group(uniqueId).SendAsync("NewWebhook", data);
    }
    public async Task JoinGroup(string uniqueId)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, uniqueId);
    }

    public async Task LeaveGroup(string uniqueId)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, uniqueId);
    }
}
