namespace SignalRIntro.Api.Interfaces;

public interface IChatClient
{
    Task ReceiveMessage(string message);
}
