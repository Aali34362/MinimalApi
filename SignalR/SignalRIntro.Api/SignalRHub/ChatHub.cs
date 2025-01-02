using Microsoft.AspNetCore.SignalR;
using SignalRIntro.Api.Interfaces;

namespace SignalRIntro.Api.SignalRHub;

public sealed class ChatHub : Hub<IChatClient>
{
    public override async Task OnConnectedAsync()
    {
        try
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} has joined");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in OnConnectedAsync: {ex.Message}");
        }
    }

    public async Task SendMessage(string message)
    {
        try
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} : {message}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
        }
    }

    public async Task<string> InvocationMessage(string message)
    {
        try
        {
            await Clients.All.ReceiveMessage($"{Context.ConnectionId} : {message}");
            return $"Message sent successfully";
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error in SendMessage: {ex.Message}");
            return ex.Message;
        }
    }
}
