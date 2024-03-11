namespace FormatDemo.TypesofLifeTimeServices;

internal class LifeTimeServices
{
}

public interface IUniqueIdGeneratorTransient
{
    string GenerateUniqueId();
}

public interface IUniqueIdGeneratorScoped
{
    string GenerateUniqueId();
}

public interface IUniqueIdGeneratorSingleton
{
    string GenerateUniqueId();
}

public class TransientServices : IUniqueIdGeneratorTransient
{
    public string GenerateUniqueId() => Guid.NewGuid().ToString();    
}

public class ScopedServices : IUniqueIdGeneratorScoped
{
    public string GenerateUniqueId() => Guid.NewGuid().ToString();    
}

public class SingletonServices : IUniqueIdGeneratorSingleton
{
    public string GenerateUniqueId() => Guid.NewGuid().ToString();
}



public interface IOperation
{
    Guid OperationId { get; }
}
public interface IOperationTransient : IOperation
{

}

public interface IOperationScoped : IOperation
{

}

public interface IOperationSingleton : IOperation
{

}

public interface IOperationSingletonInstance : IOperation
{

}

public class Opertion : IOperationTransient, IOperationScoped, IOperationSingleton, IOperationSingletonInstance
{
    public Opertion() : this(Guid.NewGuid())
    {

    }
    public Guid OperationId { get; private set; }
    public Opertion(Guid id)
    {
        OperationId = id;
    }
    //public Guid OperationId { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}

