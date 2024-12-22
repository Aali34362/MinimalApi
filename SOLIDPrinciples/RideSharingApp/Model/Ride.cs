namespace RideSharingApp.Model;

public class Ride
{
    public int Id { get; set; }
    public string? RiderName { get; set; }
    public string? DriverName { get; set; }
    public RideStatus Status { get; set; }
    public decimal Fare { get; set; }
    public DateTime RequestTime { get; set; }
}
