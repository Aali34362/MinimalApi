namespace FormatDemo.TypesofLifeTimeServices;

public class DependencyService
{
    private readonly IOperationTransient transient;
    private readonly IOperationScoped scoped;
    private readonly IOperationSingleton singleton;
    private readonly IOperationSingletonInstance singletonInstance;

    public DependencyService(IOperationScoped operationScoped, IOperationSingleton operationSingleton, IOperationSingletonInstance operationSingletonInstance, IOperationTransient operationTransient)
    {
        transient = operationTransient;
        scoped = operationScoped;
        singleton = operationSingleton;
        singletonInstance = operationSingletonInstance;
    }

    public void Write()
    {
        Console.WriteLine();
        Console.WriteLine("Fron Dependency Services");
        Console.WriteLine($"Transient - {transient.OperationId}");
        Console.WriteLine($"Scoped - {scoped.OperationId}");
        Console.WriteLine($"Singleton - {singleton.OperationId}");
        Console.WriteLine($"Singleton Instance - {singletonInstance.OperationId}");
    }
}
