namespace RideSharingApp.Services;

public interface IPaymentService
{
    void ProcessPayment(string riderName, decimal amount);
}

public class PaymentService : IPaymentService
{
    public void ProcessPayment(string riderName, decimal amount)
    {
        Console.WriteLine($"Payment of {amount:C} processed for {riderName}.");
    }
}