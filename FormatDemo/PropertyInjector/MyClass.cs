namespace FormatDemo.PropertyInjector;

public class MyClass
{
    // Public property for property injection
    public IService service { get; set; }
    public void PerformAction()
    {
        // Check if the service is null to handle scenarios where it wasn't injected
        if (service != null)
        {
            service.DoSomething();
        }
        else
        {
            Console.WriteLine("Service has not been injected.");
        }
    }
}
