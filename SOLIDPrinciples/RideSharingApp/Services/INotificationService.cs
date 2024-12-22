namespace RideSharingApp.Services;

public interface INotificationService
{
    void NotifyRider(string message);
    void NotifyDriver(string message);
}

public class NotificationService : INotificationService
{
    public void NotifyRider(string message)
    {
        Console.WriteLine($"[Rider Notification] {message}");
    }

    public void NotifyDriver(string message)
    {
        Console.WriteLine($"[Driver Notification] {message}");
    }
}