using RideSharingApp.Model;

namespace RideSharingApp.Services;

public interface IRideService
{
    void RequestRide(string riderName);
    void AcceptRide(int rideId, string driverName);
    void CompleteRide(int rideId);
    void CancelRide(int rideId);
}

public class RideService : IRideService
{
    private readonly List<Ride> _rides = new List<Ride>();
    private readonly IPaymentService _paymentService;
    private readonly INotificationService _notificationService;

    public RideService(IPaymentService paymentService, INotificationService notificationService)
    {
        _paymentService = paymentService;
        _notificationService = notificationService;
    }

    public void RequestRide(string riderName)
    {
        var ride = new Ride
        {
            Id = _rides.Count + 1,
            RiderName = riderName,
            Status = RideStatus.Requested,
            RequestTime = DateTime.Now,
            Fare = CalculateFare()
        };

        _rides.Add(ride);
        _notificationService.NotifyRider($"Ride requested successfully. Ride ID: {ride.Id}");
        Console.WriteLine($"Ride requested by {riderName}. Fare: {ride.Fare:C}");
    }

    public void AcceptRide(int rideId, string driverName)
    {
        var ride = _rides.FirstOrDefault(r => r.Id == rideId);
        if (ride == null || ride.Status != RideStatus.Requested)
        {
            Console.WriteLine("Invalid ride ID or the ride cannot be accepted.");
            return;
        }

        ride.DriverName = driverName;
        ride.Status = RideStatus.Accepted;

        _notificationService.NotifyRider($"Your ride has been accepted by {driverName}.");
        _notificationService.NotifyDriver($"You have accepted the ride for {ride.RiderName}.");
        Console.WriteLine($"Ride ID {rideId} accepted by {driverName}.");
    }

    public void CompleteRide(int rideId)
    {
        var ride = _rides.FirstOrDefault(r => r.Id == rideId);
        if (ride == null || ride.Status != RideStatus.Accepted)
        {
            Console.WriteLine("Invalid ride ID or the ride cannot be completed.");
            return;
        }

        ride.Status = RideStatus.Completed;
        _paymentService.ProcessPayment(ride.RiderName!, ride.Fare);

        _notificationService.NotifyRider("Your ride has been completed.");
        _notificationService.NotifyDriver("The ride has been successfully completed.");
        Console.WriteLine($"Ride ID {rideId} completed. Fare charged: {ride.Fare:C}");
    }

    public void CancelRide(int rideId)
    {
        var ride = _rides.FirstOrDefault(r => r.Id == rideId);
        if (ride == null || ride.Status == RideStatus.Completed)
        {
            Console.WriteLine("Invalid ride ID or the ride cannot be cancelled.");
            return;
        }

        ride.Status = RideStatus.Cancelled;

        _notificationService.NotifyRider("Your ride has been cancelled.");
        _notificationService.NotifyDriver("The ride has been cancelled.");
        Console.WriteLine($"Ride ID {rideId} cancelled.");
    }

    private decimal CalculateFare()
    {
        // Simple fare calculation based on random distance
        var random = new Random();
        var distance = random.Next(5, 20); // Random distance between 5 and 20 km
        return distance * 10; // Assume fare is 10 per km
    }
}