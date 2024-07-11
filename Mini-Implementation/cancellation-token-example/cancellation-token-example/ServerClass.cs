namespace cancellation_token_example;

public class ServerClass
{
    public static void StaticMethod(object obj)
    {
        CancellationToken ct = (CancellationToken)obj;
        Console.WriteLine("ServerClass.StaticMethod is running on another thread.");

        // Simulate work that can be canceled.
        while (!ct.IsCancellationRequested)
        {
            Thread.SpinWait(50000);
        }
        Console.WriteLine("The worker thread has been canceled. Press any key to exit.");
        Console.ReadKey(true);
    }
}
