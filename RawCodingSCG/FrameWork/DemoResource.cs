
namespace RawCodingSCG.FrameWork;

public class DemoResource : IDisposable
{
    public void Dispose()
    {
        Console.WriteLine("Closing Connection via Dispose");
    }
    public void DoWork()
    {
        Console.WriteLine("Opening Connection");
        Console.WriteLine("Doing Work");
        throw new Exception("I Broke");
        Console.WriteLine("Closing Connection");
    }
}
